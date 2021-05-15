using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.SystemTextJson;
using HttpHeader = NClient.Abstractions.HttpClients.HttpHeader;
using HttpResponse = NClient.Abstractions.HttpClients.HttpResponse;


namespace NClient.Providers.HttpClient.RestSharp
{
    public class RestSharpHttpClient : IHttpClient
    {
        private readonly IAuthenticator? _authenticator;

        public RestSharpHttpClient(IAuthenticator? authenticator = null)
        {
            _authenticator = authenticator;
        }

        public async Task<HttpResponse> ExecuteAsync(HttpRequest request, Type? bodyType = null)
        {
            var restClient = new RestClient
            {
                Authenticator = _authenticator,
            }.UseSystemTextJson();

            var restRequest = BuildRestRequest(request);
            var restResponse = await restClient.ExecuteAsync(restRequest).ConfigureAwait(false);
            return BuildResponse(request, restResponse, bodyType);
        }

        private static IRestRequest BuildRestRequest(HttpRequest request)
        {
            Enum.TryParse(request.Method.ToString(), out Method method);
            var restRequest = new RestRequest(request.Uri, method, DataFormat.Json);

            foreach (var param in request.Parameters)
            {
                restRequest.AddParameter(param.Name, param.Value!, ParameterType.QueryString);
            }

            foreach (var header in request.Headers)
            {
                restRequest.AddHeader(header.Name, header.Value);
            }

            if (request.Body is not null)
            {
                restRequest.AddJsonBody(request.Body);
            }

            return restRequest;
        }

        private static HttpResponse BuildResponse(HttpRequest request, IRestResponse restResponse, Type? bodyType = null)
        {
            var response = new HttpResponse(request)
            {
                ContentType = string.IsNullOrEmpty(restResponse.ContentType) ? null : restResponse.ContentType,
                ContentLength = restResponse.ContentLength,
                ContentEncoding = string.IsNullOrEmpty(restResponse.ContentEncoding) ? null : restResponse.ContentEncoding,
                Content = string.IsNullOrEmpty(restResponse.Content) ? null : restResponse.Content,
                StatusCode = restResponse.StatusCode,
                ResponseUri = restResponse.ResponseUri,
                Server = string.IsNullOrEmpty(restResponse.Server) ? null : restResponse.Server,
                Headers = restResponse.Headers
                    .Where(x => x.Name != null)
                    .Select(x => new HttpHeader(x.Name!, x.Value?.ToString() ?? "")).ToArray(),
                ErrorMessage = restResponse.ErrorMessage,
                ErrorException = restResponse.ErrorException,
                ProtocolVersion = restResponse.ProtocolVersion
            };

            if (bodyType is null)
                return response;
            
            if (!response.IsSuccessful)
            {
                var failureResponseType = typeof(HttpResponse<>).MakeGenericType(bodyType);
                return (HttpResponse)Activator.CreateInstance(failureResponseType, request, response, null);
            }

            var responseValue = TryParseJson(restResponse.Content, bodyType, out var deserializationException);
            if (deserializationException is not null)
                throw deserializationException!;

            var genericResponse = typeof(HttpResponse<>).MakeGenericType(bodyType);
            return (HttpResponse)Activator.CreateInstance(genericResponse, request, response, responseValue);
        }

        private static object? TryParseJson(string body, Type bodyType, out Exception? exception)
        {
            try
            {
                exception = null;
                return JsonSerializer.Deserialize(body, bodyType);
            }
            catch (Exception e)
            {
                exception = e;
                return null;
            }
        }
    }
}
