using System;
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

            foreach (var (name, value) in request.Parameters)
            {
                restRequest.AddParameter(name, value, ParameterType.QueryString);
            }

            foreach (var (name, value) in request.Headers)
            {
                restRequest.AddHeader(name, value);
            }

            if (request.Body is not null)
            {
                restRequest.AddJsonBody(request.Body);
            }

            return restRequest;
        }

        private static HttpResponse BuildResponse(IRestResponse restResponse, Type? bodyType = null)
        {
            var response = new HttpResponse(restResponse.StatusCode)
            {
                Content = string.IsNullOrEmpty(restResponse.Content) ? null : restResponse.Content,
                ContentEncoding = restResponse.ContentEncoding,
                ErrorMessage = restResponse.ErrorMessage,
                ErrorException = restResponse.ErrorException
            };

            if (bodyType is null)
                return response;

            var responseValue = JsonConvert.DeserializeObject(restResponse.Content, bodyType);
            var genericResponse = typeof(HttpResponse<>).MakeGenericType(bodyType);
            return (HttpResponse)Activator.CreateInstance(genericResponse, response, responseValue);
        }
    }
}
