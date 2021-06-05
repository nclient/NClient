using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.OptionalNClientBuilders;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client factory with custom providers.
    /// </summary>
    public class NClientStandaloneFactoryBuilder : INClientFactoryBuilder
    {
        private readonly OptionalNClientFactoryBuilder _optionalNClientFactoryBuilder;

        /// <summary>
        /// Creates the client factory with custom providers.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/> instances.</param>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/> instances.</param>
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

        /// <summary>
        /// Creates client factory.
        /// </summary>
        public INClientFactory Build()
        {
            return _optionalNClientFactoryBuilder.Build();
        }
    }
}