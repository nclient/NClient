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
        private readonly BuilderContextModificator<TRequest, TResponse> _builderContextModificator;
        private readonly IEnumerable<MethodInfo>? _selectedMethods;
        
        public NClientFactoryResilienceSetter(BuilderContextModificator<TRequest, TResponse> builderContextModificator, MethodInfo? selectedMethod) 
            : this(builderContextModificator, selectedMethod is null ? null : new[] { selectedMethod })
        {
        }
        
        public NClientFactoryResilienceSetter(BuilderContextModificator<TRequest, TResponse> builderContextModificator, IEnumerable<MethodInfo>? selectedMethods)
        {
            _builderContextModificator = builderContextModificator;
            _selectedMethods = selectedMethods;
        }

        public INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider)
        {
            _builderContextModificator.Add(context => _selectedMethods is null
                ? context.WithResiliencePolicy(resiliencePolicyProvider)
                : context.WithResiliencePolicy(_selectedMethods, resiliencePolicyProvider));
            return new NClientFactoryResilienceMethodSelector<TRequest, TResponse>(_builderContextModificator);
        }
        
        public INClientFactoryResilienceMethodSelector<TRequest, TResponse> DoNotUse()
        {
            _builderContextModificator.Add(context => _selectedMethods is null
                ? context.WithoutAllMethodsResiliencePolicy()
                : context.WithoutMethodResiliencePolicy(_selectedMethods));
            return new NClientFactoryResilienceMethodSelector<TRequest, TResponse>(_builderContextModificator);
        }
    }
}
