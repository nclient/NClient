using NClient.Abstractions;
using NClient.Abstractions.Customization;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Resilience.Providers;
using NClient.Common.Helpers;
using NClient.Customization;
using NClient.Customization.Context;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client with custom providers.
    /// </summary>
    public class NClientStandaloneBuilder<TRequest, TResponse> : INClientBuilder<TRequest, TResponse>
    {
        private readonly CustomizerContext<TRequest, TResponse> _customizerContext;
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _defaultResiliencePolicyProvider;

        public NClientStandaloneBuilder() : this(
            customizerContext: new CustomizerContext<TRequest, TResponse>(),
            defaultResiliencePolicyProvider: new NoResiliencePolicyProvider<TRequest, TResponse>())
        {
        }
        
        public NClientStandaloneBuilder(
            CustomizerContext<TRequest, TResponse> customizerContext,
            IResiliencePolicyProvider<TRequest, TResponse> defaultResiliencePolicyProvider)
        {
            _customizerContext = customizerContext;
            _defaultResiliencePolicyProvider = defaultResiliencePolicyProvider;
        }
        
        public INClientBuilderCustomizer<TInterface, TRequest, TResponse> For<TInterface>(string host)
            where TInterface : class
        {
            Ensure.IsNotNull(host, nameof(host));
            
            _customizerContext.SetHost(host);

            return new BuilderCustomizer<TInterface, TRequest, TResponse>(_customizerContext, _defaultResiliencePolicyProvider);
        }
    }
}
