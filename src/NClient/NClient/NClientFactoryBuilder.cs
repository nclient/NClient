using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;

namespace NClient
{
    public class NClientFactoryBuilder : INClientFactoryBuilder
    {
        private IHttpClientProvider? _httpClientProvider;
        private ISerializerProvider? _serializerProvider;
        private IResiliencePolicyProvider? _resiliencePolicyProvider;
        private ILoggerFactory? _loggerFactory;

        public IOptionalNClientFactoryBuilder WithCustomHttpClient(IHttpClientProvider httpClientProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));

            _httpClientProvider = httpClientProvider;
            return this;
        }

        public IOptionalNClientFactoryBuilder WithCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            _serializerProvider = serializerProvider;
            return this;
        }

        public IOptionalNClientFactoryBuilder WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            Ensure.IsNotNull(resiliencePolicyProvider, nameof(resiliencePolicyProvider));

            _resiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public IOptionalNClientFactoryBuilder WithLogging(ILoggerFactory loggerFactory)
        {
            Ensure.IsNotNull(loggerFactory, nameof(loggerFactory));

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