using System;
using System.Linq.Expressions;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Core.Helpers;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Resilience
{
    internal class NClientResilienceMethodSelector<TClient, TRequest, TResponse> : INClientResilienceMethodSelector<TClient, TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientResilienceMethodSelector(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }
        
        public INClientResilienceSetter<TClient, TRequest, TResponse> ForAllMethods()
        {
            var selectedMethods = typeof(TClient).GetInterfaceMethods();
            return new NClientResilienceSetter<TClient, TRequest, TResponse>(_builderContextModifier, selectedMethods);
        }
        
        public INClientResilienceSetter<TClient, TRequest, TResponse> ForMethod(Expression<Func<TClient, Delegate>> methodSelector)
        {
            var func = methodSelector.Compile();
            var selectedMethod = func.Invoke(default!).Method;
            return new NClientResilienceSetter<TClient, TRequest, TResponse>(_builderContextModifier, selectedMethod);
        }
    }
}
