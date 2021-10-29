using System.Net.Http;

namespace NClient.Providers.Transport.Http.System.Helpers
{
    internal class SystemHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public SystemHttpClientFactory()
        {
            _httpClient = new HttpClient(new HttpClientHandler());
        }

        public SystemHttpClientFactory(HttpMessageHandler httpMessageHandler)
        {
            _httpClient = new HttpClient(httpMessageHandler);
        }

        public SystemHttpClientFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpClient CreateClient(string name)
        {
            return _httpClient;
        }
    }
}
