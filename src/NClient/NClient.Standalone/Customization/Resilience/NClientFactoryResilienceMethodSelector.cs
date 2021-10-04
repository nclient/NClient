using System;
using System.Linq.Expressions;
using NClient.Abstractions.Customization.Resilience;
using NClient.Builders.Context;
using NClient.Core.Helpers;

namespace NClient.Customization.Resilience
{
    internal class NClientFactoryResilienceMethodSelector<TRequest, TResponse> : INClientFactoryResilienceMethodSelector<TRequest, TResponse>
    {
        private readonly CustomizerContext<TRequest, TResponse> _context;
        
        public NClientFactoryResilienceMethodSelector(CustomizerContext<TRequest, TResponse> context)
        {
            _context = context;
        }

        public INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethods()
        {
            return new NClientFactoryResilienceSetter<TRequest, TResponse>(_context, selectedMethods: null);
        }
        
        public INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethodsOf<TClient>()
        {
            var selectedMethods = typeof(TClient).GetInterfaceMethods();
            return new NClientFactoryResilienceSetter<TRequest, TResponse>(_context, selectedMethods);
        }
        
        public INClientFactoryResilienceSetter<TRequest, TResponse> ForMethodOf<TClient>(Expression<Func<TClient, Delegate>> methodSelector)
        {
            var func = methodSelector.Compile();
            var selectedMethod = func.Invoke(default!).Method;
            return new NClientFactoryResilienceSetter<TRequest, TResponse>(_context, selectedMethod);
        }
    }
}
