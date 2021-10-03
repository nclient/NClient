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
        CommonCustomizer<INClientFactoryCustomizer<TRequest, TResponse>, TRequest, TResponse>,
        INClientFactoryCustomizer<TRequest, TResponse>
    {
        private readonly string _name;
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _defaultResiliencePolicyProvider;
        
        public FactoryCustomizer(
            string name,
            CustomizerContext<TRequest, TResponse> context,
            IResiliencePolicyProvider<TRequest, TResponse> defaultResiliencePolicyProvider) 
            : base(context)
        {
            _name = name;
            _defaultResiliencePolicyProvider = defaultResiliencePolicyProvider;
        }
        
        public INClientFactoryCustomizer<TRequest, TResponse> WithCustomResilience(Action<IResiliencePolicyMethodSelector<TRequest, TResponse>> customizer)
        {
            Ensure.IsNotNull(customizer, nameof(customizer));

            customizer(new ResiliencePolicyMethodSelector<TRequest, TResponse>(Context));
            return this;
        }

        public INClientFactory Build()
        {
            return new CustomNClientFactory<TRequest, TResponse>(_name, Context, _defaultResiliencePolicyProvider);
        }
    }
}
