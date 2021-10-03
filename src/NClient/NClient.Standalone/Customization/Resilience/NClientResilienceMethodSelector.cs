using System;
using System.Linq.Expressions;
using NClient.Abstractions.Customization.Resilience;
using NClient.Builders.Context;
using NClient.Core.Helpers;

namespace NClient.Customization.Resilience
{
    public class NClientResilienceMethodSelector<TClient, TRequest, TResponse> : INClientResilienceMethodSelector<TClient, TRequest, TResponse>
    {
        private readonly CustomizerContext<TRequest, TResponse> _context;
        
        public NClientResilienceMethodSelector(CustomizerContext<TRequest, TResponse> context)
        {
            _context = context;
        }
        
        public INClientResilienceSetter<TClient, TRequest, TResponse> ForAllMethods()
        {
            var selectedMethods = typeof(TClient).GetInterfaceMethods();
            return new NClientResilienceSetter<TClient, TRequest, TResponse>(_context, selectedMethods);
        }
        
        public INClientResilienceSetter<TClient, TRequest, TResponse> ForMethod(Expression<Func<TClient, Delegate>> methodSelector)
        {
            var func = methodSelector.Compile();
            var selectedMethod = func.Invoke(default!).Method;
            return new NClientResilienceSetter<TClient, TRequest, TResponse>(_context, selectedMethod);
        }
    }
}
