using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Providers.HttpClient.RestSharp.Builders;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.SystemTextJson;
using HttpResponse = NClient.Abstractions.HttpClients.HttpResponse;

namespace NClient.Providers.HttpClient.RestSharp
{
    internal class RestSharpHttpClient : IHttpClient
    {
        private readonly IRestRequestBuilder _restRequestBuilder;
        private readonly IHttpResponseBuilder _httpResponseBuilder;
        private readonly IHttpResponsePopulater _httpResponsePopulater;
        private readonly IAuthenticator? _authenticator;

        public RestSharpHttpClient(
            IRestRequestBuilder restRequestBuilder,
            IHttpResponseBuilder httpResponseBuilder,
            IHttpResponsePopulater httpResponsePopulater,
            IAuthenticator? authenticator = null)
        {
            _restRequestBuilder = restRequestBuilder;
            _httpResponseBuilder = httpResponseBuilder;
            _httpResponsePopulater = httpResponsePopulater;
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
            var httpResponse = _httpResponseBuilder.Build(request, restResponse);
            return _httpResponsePopulater.Populate(httpResponse, bodyType, errorType);
        }
    }
}
