using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Providers.HttpClient.RestSharp.Builders;
using RestSharp;
using RestSharp.Authenticators;
using HttpResponse = NClient.Abstractions.HttpClients.HttpResponse;

namespace NClient.Providers.HttpClient.RestSharp
{
    internal class RestSharpHttpClient : IHttpClient
    {
        private readonly IRestRequestBuilder _restRequestBuilder;
        private readonly IHttpResponseBuilder _httpResponseBuilder;
        private readonly IAuthenticator? _authenticator;

        public RestSharpHttpClient(
            IRestRequestBuilder restRequestBuilder,
            IHttpResponseBuilder httpResponseBuilder,
            IAuthenticator? authenticator = null)
        {
            _restRequestBuilder = restRequestBuilder;
            _httpResponseBuilder = httpResponseBuilder;
            _authenticator = authenticator;
        }

        public async Task<HttpResponse> ExecuteAsync(HttpRequest request)
        {
            var restClient = new RestClient
            {
                Authenticator = _authenticator
            };

            var restRequest = _restRequestBuilder.Build(request);
            var restResponse = await restClient.ExecuteAsync(restRequest).ConfigureAwait(false);
            return _httpResponseBuilder.Build(request, restResponse);
        }
    }
}
