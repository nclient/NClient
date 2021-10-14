using System;
using System.Linq.Expressions;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Core.Helpers;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Resilience
{
    internal class NClientFactoryResilienceMethodSelector<TRequest, TResponse> : INClientFactoryResilienceMethodSelector<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientFactoryResilienceMethodSelector(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }

        public INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethods()
        {
            return new NClientFactoryResilienceSetter<TRequest, TResponse>(_builderContextModifier, selectedMethods: null);
        }
        
        public INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethodsOf<TClient>()
        {
            var selectedMethods = typeof(TClient).GetInterfaceMethods();
            return new NClientFactoryResilienceSetter<TRequest, TResponse>(_builderContextModifier, selectedMethods);
        }
        
        public INClientFactoryResilienceSetter<TRequest, TResponse> ForMethodOf<TClient>(Expression<Func<TClient, Delegate>> methodSelector)
        {
            var func = methodSelector.Compile();
            var selectedMethod = func.Invoke(default!).Method;
            return new NClientFactoryResilienceSetter<TRequest, TResponse>(_builderContextModifier, selectedMethod);
        }
    }
}
