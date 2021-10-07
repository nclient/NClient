using System;
using System.Linq.Expressions;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Core.Helpers;
using NClient.Standalone.Builders.Context;

namespace NClient.Standalone.Configuration.Resilience
{
    internal class NClientResilienceMethodSelector<TClient, TRequest, TResponse> : INClientResilienceMethodSelector<TClient, TRequest, TResponse>
    {
        private readonly BuilderContextModificator<TRequest, TResponse> _builderContextModificator;
        
        public NClientResilienceMethodSelector(BuilderContextModificator<TRequest, TResponse> builderContextModificator)
        {
            _builderContextModificator = builderContextModificator;
        }
        
        public INClientResilienceSetter<TClient, TRequest, TResponse> ForAllMethods()
        {
            var selectedMethods = typeof(TClient).GetInterfaceMethods();
            return new NClientResilienceSetter<TClient, TRequest, TResponse>(_builderContextModificator, selectedMethods);
        }
        
        public INClientResilienceSetter<TClient, TRequest, TResponse> ForMethod(Expression<Func<TClient, Delegate>> methodSelector)
        {
            var func = methodSelector.Compile();
            var selectedMethod = func.Invoke(default!).Method;
            return new NClientResilienceSetter<TClient, TRequest, TResponse>(_builderContextModificator, selectedMethod);
        }
    }
}
