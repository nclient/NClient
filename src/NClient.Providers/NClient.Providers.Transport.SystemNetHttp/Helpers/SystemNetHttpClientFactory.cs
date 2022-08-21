using System.Net.Http;
using System.Threading;

namespace NClient.Providers.Transport.SystemNetHttp.Helpers
{
    internal class SystemNetHttpClientFactory : IHttpClientFactory
    {
        #if NETCOREAPP2_1_OR_GREATER
        private const bool DisposeHandler = false;
        private static readonly HttpClientHandler? HttpHandler = new SocketsHttpHandler();
        #else
        private const bool DisposeHandler = true;
        private static readonly HttpClientHandler? HttpHandler = new();
        #endif
        
        private readonly HttpClient _httpClient;

        public SystemNetHttpClientFactory()
        {
            _httpClient = new HttpClient(HttpHandler, DisposeHandler)
            {
                Timeout = Timeout.InfiniteTimeSpan
            };
        }
        
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
