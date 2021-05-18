using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Providers.HttpClient.RestSharp.Internals;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.SystemTextJson;
using HttpHeader = NClient.Abstractions.HttpClients.HttpHeader;
using HttpResponse = NClient.Abstractions.HttpClients.HttpResponse;


namespace NClient.Providers.HttpClient.RestSharp
{
    public class RestSharpHttpClient : IHttpClient
    {
        private readonly RestRequestBuilder _restRequestBuilder;
        private readonly HttpResponseBuilder _httpResponseBuilder;
        private readonly IAuthenticator? _authenticator;

        public RestSharpHttpClient(IAuthenticator? authenticator = null)
        {
            _restRequestBuilder = new RestRequestBuilder();
            _httpResponseBuilder = new HttpResponseBuilder();
            _authenticator = authenticator;
        }

        public async Task<HttpResponse> ExecuteAsync(HttpRequest request, Type? bodyType = null, Type? errorType = null)
        {
            var restClient = new RestClient
            {
                Authenticator = _authenticator,
            }.UseSystemTextJson();

            var restRequest = _restRequestBuilder.Build(request);
            var restResponse = await restClient.ExecuteAsync(restRequest).ConfigureAwait(false);
            return _httpResponseBuilder.Build(request, restResponse, bodyType, errorType);
        }
    }
}
