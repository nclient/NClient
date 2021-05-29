using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;

namespace NClient
{
    public class NClientStandaloneFactoryBuilder : INClientFactoryBuilder
    {
        private IHttpClientProvider _httpClientProvider;
        private ISerializerProvider _serializerProvider;
        private IResiliencePolicyProvider? _resiliencePolicyProvider;
        private ILoggerFactory? _loggerFactory;

        public NClientStandaloneFactoryBuilder(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            _httpClientProvider = httpClientProvider;
            _serializerProvider = serializerProvider;
        }

        public IOptionalNClientFactoryBuilder WithCustomHttpClient(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
            return this;
        }

        public IOptionalNClientFactoryBuilder WithCustomSerializer(ISerializerProvider serializerProvider)
        {
            _serializerProvider = serializerProvider;
            return this;
        }

        public IOptionalNClientFactoryBuilder WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            _resiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public IOptionalNClientFactoryBuilder WithLogging(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            return this;
        }

        public INClientFactory Build()
        {
            return new NClientStandaloneFactory(
                _httpClientProvider,
                _serializerProvider,
                _resiliencePolicyProvider,
                _loggerFactory);
        }
    }
}