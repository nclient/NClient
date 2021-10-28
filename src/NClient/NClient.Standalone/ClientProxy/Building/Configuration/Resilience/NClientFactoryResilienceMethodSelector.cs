using System;
using System.Linq.Expressions;
using System.Reflection;
using NClient.Core.Helpers;
using NClient.Core.Helpers.EqualityComparers;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Resilience
{
    internal class NClientFactoryResilienceMethodSelector<TRequest, TResponse> : INClientFactoryResilienceMethodSelector<TRequest, TResponse>
    {
        private readonly MethodInfoEqualityComparer _methodInfoEqualityComparer = new();
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientFactoryResilienceMethodSelector(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }

        public INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethods()
        {
            return new NClientFactoryResilienceSetter<TRequest, TResponse>(
                _builderContextModifier, 
                methodPredicate: (_, _) => true);
        }
        
        public INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethodsOf<TClient>()
        {
            // TODO: test it
            return new NClientFactoryResilienceSetter<TRequest, TResponse>(
                _builderContextModifier, 
                methodPredicate: (methodInfo, _) => methodInfo.DeclaringType == typeof(TClient));
        }
        
        public INClientFactoryResilienceSetter<TRequest, TResponse> ForMethodOf<TClient>(Expression<Func<TClient, Delegate>> methodSelector)
        {
            var func = methodSelector.Compile();
            var selectedMethod = func.Invoke(default!).Method;
            return new NClientFactoryResilienceSetter<TRequest, TResponse>(
                _builderContextModifier, 
                methodPredicate: (methodInfo, _) => _methodInfoEqualityComparer.Equals(methodInfo, selectedMethod));
        }
        
        public INClientFactoryResilienceSetter<TRequest, TResponse> ForMethodsThat(Func<MethodInfo, IRequest, bool> predicate)
        {
            return new NClientFactoryResilienceSetter<TRequest, TResponse>(
                _builderContextModifier, 
                predicate);
        }
    }
}
