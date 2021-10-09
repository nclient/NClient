using System.Collections.Generic;
using System.Reflection;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Abstractions.Resilience;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Resilience
{
    internal class NClientResilienceSetter<TClient, TRequest, TResponse> : INClientResilienceSetter<TClient, TRequest, TResponse>
    {
        private readonly BuilderContextModificator<TRequest, TResponse> _builderContextModificator;
        private readonly IEnumerable<MethodInfo>? _selectedMethods;

        public NClientResilienceSetter(
            BuilderContextModificator<TRequest, TResponse> builderContextModificator, 
            MethodInfo? selectedMethod) 
            : this(builderContextModificator, selectedMethod is null ? null : new[] { selectedMethod })
        {
        }
        
        public NClientResilienceSetter(
            BuilderContextModificator<TRequest, TResponse> builderContextModificator, 
            IEnumerable<MethodInfo>? selectedMethods)
        {
            _builderContextModificator = builderContextModificator;
            _selectedMethods = selectedMethods;
        }

        INClientResilienceMethodSelector<TClient, TRequest, TResponse> INClientResilienceSetter<TClient, TRequest, TResponse>.Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider)
        {
            _builderContextModificator.Add(context => _selectedMethods is null 
                ? context.WithResiliencePolicy(resiliencePolicyProvider) 
                : context.WithResiliencePolicy(_selectedMethods, resiliencePolicyProvider));
            return new NClientResilienceMethodSelector<TClient, TRequest, TResponse>(_builderContextModificator);
        }
        
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> INClientResilienceSetter<TClient, TRequest, TResponse>.DoNotUse()
        {
            _builderContextModificator.Add(context => _selectedMethods is null
                ? context.WithoutAllMethodsResiliencePolicy()
                : context.WithoutMethodResiliencePolicy(_selectedMethods));
            return new NClientResilienceMethodSelector<TClient, TRequest, TResponse>(_builderContextModificator);
        }
    }
}
