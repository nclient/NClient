using System.Net.Http;

namespace NClient.Providers.Transport.Http.System.Stubs
{
    internal class StubHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public StubHttpClientFactory()
        {
            _httpClient = new HttpClient(new HttpClientHandler());
        }

        public StubHttpClientFactory(HttpMessageHandler httpMessageHandler)
        {
            _httpClient = new HttpClient(httpMessageHandler);
        }

        public StubHttpClientFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpClient CreateClient(string name)
        {
            return _httpClient;
        }
    }
}
