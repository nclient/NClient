using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.OptionalNClientBuilders;

namespace NClient
{
    public class NClientStandaloneFactoryBuilder : INClientFactoryBuilder, IOptionalNClientFactoryBuilder
    {
        private readonly OptionalNClientFactoryBuilder _optionalNClientFactoryBuilder;

        public NClientStandaloneFactoryBuilder(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider)
        {
            _optionalNClientFactoryBuilder = new OptionalNClientFactoryBuilder(httpClientProvider, serializerProvider);
        }

        public IOptionalNClientFactoryBuilder WithCustomHttpClient(IHttpClientProvider httpClientProvider)
        {
            return _optionalNClientFactoryBuilder.WithCustomHttpClient(httpClientProvider);
        }

        public IOptionalNClientFactoryBuilder WithCustomSerializer(ISerializerProvider serializerProvider)
        {
            return _optionalNClientFactoryBuilder.WithCustomSerializer(serializerProvider);
        }

        public IOptionalNClientFactoryBuilder WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            return _optionalNClientFactoryBuilder.WithResiliencePolicy(resiliencePolicyProvider);
        }

        public IOptionalNClientFactoryBuilder WithLogging(ILoggerFactory loggerFactory)
        {
            return _optionalNClientFactoryBuilder.WithLogging(loggerFactory);
        }

        public INClientFactory Build()
        {
            return _optionalNClientFactoryBuilder.Build();
        }
    }
}