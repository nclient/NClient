using System.Collections.Generic;
using System.Reflection;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Abstractions.Resilience;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Resilience
{
    // TODO: Need tests
    internal class NClientFactoryResilienceSetter<TRequest, TResponse> : INClientFactoryResilienceSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        private readonly IEnumerable<MethodInfo>? _selectedMethods;
        
        public NClientFactoryResilienceSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier, MethodInfo? selectedMethod) 
            : this(builderContextModifier, selectedMethod is null ? null : new[] { selectedMethod })
        {
        }
        
        public NClientFactoryResilienceSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier, IEnumerable<MethodInfo>? selectedMethods)
        {
            _builderContextModifier = builderContextModifier;
            _selectedMethods = selectedMethods;
        }

        public INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider)
        {
            _builderContextModifier.Add(context => _selectedMethods is null
                ? context.WithResiliencePolicy(resiliencePolicyProvider)
                : context.WithResiliencePolicy(_selectedMethods, resiliencePolicyProvider));
            return new NClientFactoryResilienceMethodSelector<TRequest, TResponse>(_builderContextModifier);
        }
        
        public INClientFactoryResilienceMethodSelector<TRequest, TResponse> DoNotUse()
        {
            _builderContextModifier.Add(context => _selectedMethods is null
                ? context.WithoutAllMethodsResiliencePolicy()
                : context.WithoutMethodResiliencePolicy(_selectedMethods));
            return new NClientFactoryResilienceMethodSelector<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
