using System.Collections.Generic;
using System.Reflection;
using NClient.Abstractions.Customization.Resilience;
using NClient.Abstractions.Resilience;
using NClient.Customization.Context;

namespace NClient.Customization.Resilience
{
    public class ResiliencePolicyProviderSetter<TClient, TRequest, TResponse> : ResiliencePolicyProviderSetter<TRequest, TResponse>, IResiliencePolicyProviderSetter<TClient, TRequest, TResponse>
    {
        public ResiliencePolicyProviderSetter(CustomizerContext<TRequest, TResponse> context, MethodInfo? selectedMethod) : base(context, selectedMethod)
        {
        }
        
        public ResiliencePolicyProviderSetter(CustomizerContext<TRequest, TResponse> context, IEnumerable<MethodInfo> selectedMethods) : base(context, selectedMethods)
        {
        }

        IResiliencePolicyMethodSelector<TClient, TRequest, TResponse> IResiliencePolicyProviderSetter<TClient, TRequest, TResponse>.Use(IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            Use(provider);
            
            return new ResiliencePolicyMethodSelector<TClient, TRequest, TResponse>(Context);
        }
    }    
    
    public class ResiliencePolicyProviderSetter<TRequest, TResponse> : IResiliencePolicyProviderSetter<TRequest, TResponse>
    {
        private readonly IEnumerable<MethodInfo>? _selectedMethods;
        
        protected readonly CustomizerContext<TRequest, TResponse> Context;
        
        public ResiliencePolicyProviderSetter(CustomizerContext<TRequest, TResponse> context, MethodInfo? selectedMethod) 
            : this(context, selectedMethod is null ? null : new[] { selectedMethod })
        {
        }
        
        public ResiliencePolicyProviderSetter(CustomizerContext<TRequest, TResponse> context, IEnumerable<MethodInfo>? selectedMethods)
        {
            Context = context;
            _selectedMethods = selectedMethods;
        }

        public IResiliencePolicyMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            if (_selectedMethods is null) 
                Context.SetResiliencePolicy(provider);
            else
                Context.SetResiliencePolicy(_selectedMethods, provider);
            
            return new ResiliencePolicyMethodSelector<TRequest, TResponse>(Context);
        }
    }
}
