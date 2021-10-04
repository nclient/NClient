using System.Collections.Generic;
using System.Reflection;
using NClient.Abstractions.Customization.Resilience;
using NClient.Abstractions.Resilience;
using NClient.Builders.Context;

namespace NClient.Customization.Resilience
{
    internal class NClientFactoryResilienceSetter<TRequest, TResponse> : INClientFactoryResilienceSetter<TRequest, TResponse>
    {
        private readonly IEnumerable<MethodInfo>? _selectedMethods;
        
        protected readonly CustomizerContext<TRequest, TResponse> Context;
        
        public NClientFactoryResilienceSetter(CustomizerContext<TRequest, TResponse> context, MethodInfo? selectedMethod) 
            : this(context, selectedMethod is null ? null : new[] { selectedMethod })
        {
        }
        
        public NClientFactoryResilienceSetter(CustomizerContext<TRequest, TResponse> context, IEnumerable<MethodInfo>? selectedMethods)
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
    }
}
