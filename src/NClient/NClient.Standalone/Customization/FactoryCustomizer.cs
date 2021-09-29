using System;
using NClient.Abstractions;
using NClient.Abstractions.Customization;
using NClient.Abstractions.Customization.Resilience;
using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using NClient.Customization.Context;
using NClient.Customization.Resilience;

namespace NClient.Customization
{
    internal class FactoryCustomizer<TRequest, TResponse> :
        CommonCustomizer<INClientFactoryCustomizer<TRequest, TResponse>, INClientFactory, TRequest, TResponse>,
        INClientFactoryCustomizer<TRequest, TResponse>
    {
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _defaultResiliencePolicyProvider;
        
        public FactoryCustomizer(
            CustomizerContext<TRequest, TResponse> context,
            IResiliencePolicyProvider<TRequest, TResponse> defaultResiliencePolicyProvider) 
            : base(context)
        {
            _defaultResiliencePolicyProvider = defaultResiliencePolicyProvider;
        }
        
        public INClientFactoryCustomizer<TRequest, TResponse> WithCustomResilience(Action<IResiliencePolicyMethodSelector<TRequest, TResponse>> customizer)
        {
            Ensure.IsNotNull(customizer, nameof(customizer));

            customizer(new ResiliencePolicyMethodSelector<TRequest, TResponse>(Context));
            return this;
        }

        public override INClientFactory Build()
        {
            return new NClientStandaloneFactory<TRequest, TResponse>(Context, _defaultResiliencePolicyProvider);
        }
    }
}
