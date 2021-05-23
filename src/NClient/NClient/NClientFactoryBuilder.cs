using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient
{
    public class NClientFactoryBuilder : INClientFactoryBuilder
    {
        private IHttpClientProvider? _httpClientProvider;
        private ISerializerProvider? _serializerProvider;
        private IResiliencePolicyProvider? _resiliencePolicyProvider;
        private ILoggerFactory? _loggerFactory;

        public INClientFactoryBuilder WithCustomHttpClient(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
            return this;
        }

        public INClientFactoryBuilder WithCustomSerializer(ISerializerProvider serializerProvider)
        {
            _serializerProvider = serializerProvider;
            return this;
        }

        public INClientFactoryBuilder WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            _resiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public INClientFactoryBuilder WithLogging(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            return this;
        }

        public INClientFactory Build()
        {
            return new NClientFactory(
                _httpClientProvider,
                _serializerProvider,
                _resiliencePolicyProvider,
                _loggerFactory);
        }
    }
}