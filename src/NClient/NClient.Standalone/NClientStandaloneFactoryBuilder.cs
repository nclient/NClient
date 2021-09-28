using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Customizers;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client factory with custom providers.
    /// </summary>
    public class NClientStandaloneFactoryBuilder : INClientFactoryBuilder
    {
        private readonly FactoryCustomizer _optionalFactoryBuilder;

        /// <summary>
        /// Creates the client factory with custom providers.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/> instances.</param>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/> instances.</param>
        public NClientStandaloneFactoryBuilder(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider)
        {
            _optionalFactoryBuilder = new FactoryCustomizer(httpClientProvider, serializerProvider);
        }
        
        public INClientFactoryCustomizer Use(string name)
        {
            return _optionalFactoryBuilder;
        }
    }
}
