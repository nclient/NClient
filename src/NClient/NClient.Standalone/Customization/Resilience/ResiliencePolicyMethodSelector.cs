using System;
using System.Linq.Expressions;
using NClient.Abstractions.Customization.Resilience;
using NClient.Core.Helpers;
using NClient.Customization.Context;

namespace NClient.Customization.Resilience
{
    public class ResiliencePolicyMethodSelector<TClient, TRequest, TResponse> : IResiliencePolicyMethodSelector<TClient, TRequest, TResponse>
    {
        private readonly CustomizerContext<TRequest, TResponse> _context;
        
        public ResiliencePolicyMethodSelector(CustomizerContext<TRequest, TResponse> context)
        {
            _context = context;
        }
        
        public IResiliencePolicyProviderSetter<TClient, TRequest, TResponse> ForAllMethods()
        {
            var selectedMethods = typeof(TClient).GetInterfaceMethods();
            return new ResiliencePolicyProviderSetter<TClient, TRequest, TResponse>(_context, selectedMethods);
        }
        
        public IResiliencePolicyProviderSetter<TClient, TRequest, TResponse> ForMethod(Expression<Func<TClient, Delegate>> methodSelector)
        {
            var func = methodSelector.Compile();
            var selectedMethod = func.Invoke(default!).Method;
            return new ResiliencePolicyProviderSetter<TClient, TRequest, TResponse>(_context, selectedMethod);
        }
    }
    
    public class ResiliencePolicyMethodSelector<TRequest, TResponse> : IResiliencePolicyMethodSelector<TRequest, TResponse>
    {
        private readonly CustomizerContext<TRequest, TResponse> _context;
        
        public ResiliencePolicyMethodSelector(CustomizerContext<TRequest, TResponse> context)
        {
            _context = context;
        }

        public IResiliencePolicyProviderSetter<TRequest, TResponse> ForAllMethods()
        {
            return new ResiliencePolicyProviderSetter<TRequest, TResponse>(_context, selectedMethods: null);
        }
        
        public IResiliencePolicyProviderSetter<TRequest, TResponse> ForAllMethodsOf<TClient>()
        {
            var selectedMethods = typeof(TClient).GetInterfaceMethods();
            return new ResiliencePolicyProviderSetter<TRequest, TResponse>(_context, selectedMethods);
        }
        
        public IResiliencePolicyProviderSetter<TRequest, TResponse> ForMethodOf<TClient>(Expression<Func<TClient, Delegate>> methodSelector)
        {
            var func = methodSelector.Compile();
            var selectedMethod = func.Invoke(default!).Method;
            return new ResiliencePolicyProviderSetter<TRequest, TResponse>(_context, selectedMethod);
        }
    }
}
