using System.Net.Http;
using NClient.Abstractions.HttpClients;

namespace NClient.Providers.HttpClient.System
{
    public class SystemHttpClientProvider : IHttpClientProvider
    {
        private readonly SystemHttpClient _systemHttpClient;

        public SystemHttpClientProvider()
        {
            _systemHttpClient = new SystemHttpClient(new StubHttpClientFactory(new HttpClientHandler()));
        }

        public SystemHttpClientProvider(HttpMessageHandler httpMessageHandler)
        {
            _systemHttpClient = new SystemHttpClient(new StubHttpClientFactory(httpMessageHandler));
        }

        public SystemHttpClientProvider(global::System.Net.Http.HttpClient httpClient)
        {
            _systemHttpClient = new SystemHttpClient(new StubHttpClientFactory(httpClient));
        }

        public SystemHttpClientProvider(IHttpClientFactory httpClientFactory)
        {
            _systemHttpClient = new SystemHttpClient(httpClientFactory);
        }

        public IHttpClient Create()
        {
            return _systemHttpClient;
        }
    }
}
