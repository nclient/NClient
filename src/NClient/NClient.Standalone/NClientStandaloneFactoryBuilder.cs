using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Core.Resilience;
using NClient.Customizers;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client factory with custom providers.
    /// </summary>
    public class NClientStandaloneFactoryBuilder<TRequest, TResponse> : INClientFactoryBuilder<TRequest, TResponse>
    {
        private readonly FactoryCustomizer<TRequest, TResponse> _optionalFactoryBuilder;

        /// <summary>
        /// Creates the client factory with custom providers.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/> instances.</param>
        /// <param name="httpMessageBuilderProvider">The provider that can create instances of <see cref="IHttpMessageBuilder"/> instances.</param>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/> instances.</param>
        public NClientStandaloneFactoryBuilder(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            ISerializerProvider serializerProvider)
            : this(httpClientProvider,
                httpMessageBuilderProvider,
                new DefaultMethodResiliencePolicyProvider<TResponse>(
                    new DefaultResiliencePolicyProvider<TResponse>()),
                serializerProvider)
        {
        }
        
        internal NClientStandaloneFactoryBuilder(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IMethodResiliencePolicyProvider<TResponse> methodResiliencePolicyProvider,
            ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(httpMessageBuilderProvider, nameof(httpMessageBuilderProvider));
            Ensure.IsNotNull(methodResiliencePolicyProvider, nameof(methodResiliencePolicyProvider));
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            
            _optionalFactoryBuilder = new FactoryCustomizer<TRequest, TResponse>(
                httpClientProvider, 
                httpMessageBuilderProvider,
                methodResiliencePolicyProvider,
                serializerProvider);
        }
        
        public INClientFactoryCustomizer<TRequest, TResponse> Use(string name)
        {
            return _optionalFactoryBuilder;
        }
    }
}
