using System.Collections.Generic;
using System.Reflection;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Abstractions.Resilience;
using NClient.Standalone.Builders.Context;

namespace NClient.Standalone.Configuration.Resilience
{
    // TODO: Need tests
    internal class NClientFactoryResilienceSetter<TRequest, TResponse> : INClientFactoryResilienceSetter<TRequest, TResponse>
    {
        private readonly IEnumerable<MethodInfo>? _selectedMethods;
        
        protected readonly BuilderContext<TRequest, TResponse> Context;
        
        public NClientFactoryResilienceSetter(BuilderContext<TRequest, TResponse> context, MethodInfo? selectedMethod) 
            : this(context, selectedMethod is null ? null : new[] { selectedMethod })
        {
        }
        
        public NClientFactoryResilienceSetter(BuilderContext<TRequest, TResponse> context, IEnumerable<MethodInfo>? selectedMethods)
        {
            Context = context;
            _selectedMethods = selectedMethods;
        }

        public INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider)
        {
            if (_selectedMethods is null) 
                Context.SetResiliencePolicy(resiliencePolicyProvider);
            else
                Context.SetResiliencePolicy(_selectedMethods, resiliencePolicyProvider);
            
            return new NClientFactoryResilienceMethodSelector<TRequest, TResponse>(Context);
        }
        
        public INClientFactoryResilienceMethodSelector<TRequest, TResponse> DoNotUse()
        {
            if (_selectedMethods is null) 
                Context.ClearAllMethodsResiliencePolicy();
            else
                Context.ClearMethodResiliencePolicy(_selectedMethods);
            
            return new NClientFactoryResilienceMethodSelector<TRequest, TResponse>(Context);
        }
    }
}
