using System.Net.Http;

namespace NClient.Providers.Transport.SystemNetHttp.Helpers
{
    internal class SystemNetHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public SystemNetHttpClientFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpClient CreateClient(string name)
        {
            return _httpClient;
        }
    }
}
