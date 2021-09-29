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
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/>.</param>
        /// <param name="httpMessageBuilderProvider">The provider that can create instances of <see cref="IHttpMessageBuilder"/>.</param>
        /// <param name="httpClientExceptionFactory">The factory that can create instances of <see cref="HttpClientException"/>.</param>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        public NClientStandaloneFactoryBuilder(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory,
            ISerializerProvider serializerProvider)
            : this(
                httpClientProvider,
                httpMessageBuilderProvider,
                httpClientExceptionFactory,
                new DefaultMethodResiliencePolicyProvider<TRequest, TResponse>(
                    new DefaultResiliencePolicyProvider<TRequest, TResponse>()),
                serializerProvider)
        {
        }
        
        internal NClientStandaloneFactoryBuilder(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(httpMessageBuilderProvider, nameof(httpMessageBuilderProvider));
            Ensure.IsNotNull(httpClientExceptionFactory, nameof(httpClientExceptionFactory));
            Ensure.IsNotNull(methodResiliencePolicyProvider, nameof(methodResiliencePolicyProvider));
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            
            _optionalFactoryBuilder = new FactoryCustomizer<TRequest, TResponse>(
                httpClientProvider, 
                httpMessageBuilderProvider,
                httpClientExceptionFactory,
                methodResiliencePolicyProvider,
                serializerProvider);
        }
        
        public INClientFactoryCustomizer<TRequest, TResponse> Use(string name)
        {
            return _optionalFactoryBuilder;
        }
    }
}
