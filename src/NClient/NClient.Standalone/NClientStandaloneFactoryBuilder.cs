using NClient.Abstractions;
using NClient.Abstractions.Customization;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Resilience.Providers;
using NClient.Customization;
using NClient.Customization.Context;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client factory with custom providers.
    /// </summary>
    public class NClientStandaloneFactoryBuilder<TRequest, TResponse> : INClientFactoryBuilder<TRequest, TResponse>
    {
        private readonly CustomizerContext<TRequest, TResponse> _customizerContext;
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _defaultResiliencePolicyProvider;

        public NClientStandaloneFactoryBuilder() : this(
            customizerContext: new CustomizerContext<TRequest, TResponse>(),
            defaultResiliencePolicyProvider: new NoResiliencePolicyProvider<TRequest, TResponse>())
        {
        }
        
        public NClientStandaloneFactoryBuilder(
            CustomizerContext<TRequest, TResponse> customizerContext,
            IResiliencePolicyProvider<TRequest, TResponse> defaultResiliencePolicyProvider)
        {
            _customizerContext = customizerContext;
            _defaultResiliencePolicyProvider = defaultResiliencePolicyProvider;
        }
        
        public INClientFactoryCustomizer<TRequest, TResponse> For(string factoryName)
        {
            return new FactoryCustomizer<TRequest, TResponse>(factoryName, _customizerContext, _defaultResiliencePolicyProvider);
        }
    }
}
