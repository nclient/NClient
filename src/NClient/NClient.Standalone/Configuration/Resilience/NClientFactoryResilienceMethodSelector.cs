using System;
using System.Linq.Expressions;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Core.Helpers;
using NClient.Standalone.Builders.Context;

namespace NClient.Standalone.Configuration.Resilience
{
    internal class NClientFactoryResilienceMethodSelector<TRequest, TResponse> : INClientFactoryResilienceMethodSelector<TRequest, TResponse>
    {
        private readonly BuilderContext<TRequest, TResponse> _context;
        
        public NClientFactoryResilienceMethodSelector(BuilderContext<TRequest, TResponse> context)
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
