using System.Collections.Generic;
using System.Reflection;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Abstractions.Resilience;
using NClient.Resilience;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Resilience
{
    internal class NClientResilienceSetter<TClient, TRequest, TResponse> : INClientResilienceSetter<TClient, TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        private readonly IEnumerable<MethodInfo>? _selectedMethods;

        public NClientResilienceSetter(
            BuilderContextModifier<TRequest, TResponse> builderContextModifier, 
            MethodInfo? selectedMethod) 
            : this(builderContextModifier, selectedMethod is null ? null : new[] { selectedMethod })
        {
        }
        
        public NClientResilienceSetter(
            BuilderContextModifier<TRequest, TResponse> builderContextModifier, 
            IEnumerable<MethodInfo>? selectedMethods)
        {
            _builderContextModifier = builderContextModifier;
            _selectedMethods = selectedMethods;
        }

        INClientResilienceMethodSelector<TClient, TRequest, TResponse> INClientResilienceSetter<TClient, TRequest, TResponse>.Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider)
        {
            _builderContextModifier.Add(context => _selectedMethods is null 
                ? context.WithoutResiliencePolicy().WithResiliencePolicy(new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(resiliencePolicyProvider)) 
                : context.WithResiliencePolicy(_selectedMethods, resiliencePolicyProvider));
            return new NClientResilienceMethodSelector<TClient, TRequest, TResponse>(_builderContextModifier);
        }
        
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> INClientResilienceSetter<TClient, TRequest, TResponse>.DoNotUse()
        {
            _builderContextModifier.Add(context => _selectedMethods is null
                ? context.WithoutResiliencePolicy()
                : context.WithoutMethodResiliencePolicy(_selectedMethods));
            return new NClientResilienceMethodSelector<TClient, TRequest, TResponse>(_builderContextModifier);
        }
    }
}
