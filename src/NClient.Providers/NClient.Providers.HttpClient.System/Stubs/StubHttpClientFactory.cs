using System.Net.Http;

namespace NClient.Providers.HttpClient.System.Stubs
{
    internal class StubHttpClientFactory : IHttpClientFactory
    {
        private readonly global::System.Net.Http.HttpClient _httpClient;

        public StubHttpClientFactory()
        {
            _httpClient = new global::System.Net.Http.HttpClient(new HttpClientHandler());
        }

        public StubHttpClientFactory(HttpMessageHandler httpMessageHandler)
        {
            _httpClient = new global::System.Net.Http.HttpClient(httpMessageHandler);
        }

        public StubHttpClientFactory(global::System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public global::System.Net.Http.HttpClient CreateClient(string name)
        {
            return _httpClient;
        }
    }
}
