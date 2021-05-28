using System;
using System.Net.Http;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
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
            Ensure.IsNotNull(httpMessageHandler, nameof(httpMessageHandler));
            
            _httpClientFactory = () => new StubHttpClientFactory(httpMessageHandler);
        }

        public SystemHttpClientProvider(global::System.Net.Http.HttpClient httpClient)
        {
            Ensure.IsNotNull(httpClient, nameof(httpClient));
            
            _httpClientFactory = () => new StubHttpClientFactory(httpClient);
        }

        public SystemHttpClientProvider(IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));
            
            _httpClientFactory = () => httpClientFactory;
            _httpClientName = httpClientName;
        }

        public IHttpClient Create(ISerializer serializer)
        {
            Ensure.IsNotNull(serializer, nameof(serializer));
            
            return new SystemHttpClient(serializer, _httpClientFactory.Invoke(), _httpClientName);
        }
    }
}
