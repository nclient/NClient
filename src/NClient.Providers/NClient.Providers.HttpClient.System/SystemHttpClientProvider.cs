using System;
using System.Net.Http;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.HttpClient.System.Internals;

namespace NClient.Providers.HttpClient.System
{
    public class SystemHttpClientProvider : IHttpClientProvider
    {
        private readonly Func<IHttpClientFactory> _httpClientFactory;
        private readonly string? _httpClientName;

        public SystemHttpClientProvider()
        {
            _httpClientFactory = () => new StubHttpClientFactory(new HttpClientHandler());
        }

        public SystemHttpClientProvider(HttpMessageHandler httpMessageHandler)
        {
            _httpClientFactory = () => new StubHttpClientFactory(httpMessageHandler);
        }

        public SystemHttpClientProvider(global::System.Net.Http.HttpClient httpClient)
        {
            _httpClientFactory = () => new StubHttpClientFactory(httpClient);
        }

        public SystemHttpClientProvider(IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            _httpClientFactory = () => httpClientFactory;
            _httpClientName = httpClientName;
        }

        public IHttpClient Create(ISerializer serializer)
        {
            return new SystemHttpClient(serializer, _httpClientFactory.Invoke(), _httpClientName);
        }
    }
}
