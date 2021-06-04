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
    public class NClientStandaloneFactoryBuilder : INClientFactoryBuilder, IOptionalNClientFactoryBuilder
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

        /// <summary>
        /// Sets custom <see cref="IHttpClientProvider"/> used to create instances of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/> instances.</param>
        public IOptionalNClientFactoryBuilder WithCustomHttpClient(IHttpClientProvider httpClientProvider)
        {
            return _optionalNClientFactoryBuilder.WithCustomHttpClient(httpClientProvider);
        }

        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/> instances.</param>
        public IOptionalNClientFactoryBuilder WithCustomSerializer(ISerializerProvider serializerProvider)
        {
            return _optionalNClientFactoryBuilder.WithCustomSerializer(serializerProvider);
        }

        /// <summary>
        /// Sets custom <see cref="IResiliencePolicyProvider"/> used to create instances of <see cref="IResiliencePolicy"/>.
        /// </summary>
        /// <param name="resiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/> instances.</param>
        public IOptionalNClientFactoryBuilder WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            return _optionalNClientFactoryBuilder.WithResiliencePolicy(resiliencePolicyProvider);
        }

        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
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