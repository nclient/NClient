using System;
using System.Linq;
using System.Threading.Tasks;
using NClient.Providers.HttpClient.Abstractions;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

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
                Authenticator = _authenticator
            };
            var restRequest = BuildRestRequest(request);
            var restResponse = await restClient.ExecuteAsync(restRequest).ConfigureAwait(false);
            return BuildResponse(restResponse, bodyType);
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

        private static HttpResponse BuildResponse(IRestResponse restResponse, Type? bodyType = null)
        {
            var response = new HttpResponse
            {
                ContentType = string.IsNullOrEmpty(restResponse.ContentType) ? null : restResponse.ContentType,
                ContentLength = restResponse.ContentLength,
                ContentEncoding = string.IsNullOrEmpty(restResponse.ContentEncoding) ? null : restResponse.ContentEncoding,
                Content = string.IsNullOrEmpty(restResponse.Content) ? null : restResponse.Content,
                StatusCode = restResponse.StatusCode,
                StatusDescription = string.IsNullOrEmpty(restResponse.StatusDescription) ? null : restResponse.StatusDescription,
                RawBytes = restResponse.RawBytes,
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

            var responseValue = JsonConvert.DeserializeObject(restResponse.Content, bodyType);
            var genericResponse = typeof(HttpResponse<>).MakeGenericType(bodyType);
            return (HttpResponse)Activator.CreateInstance(genericResponse, response, responseValue);
        }
    }
}
