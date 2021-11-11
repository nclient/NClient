using System;
using System.Linq.Expressions;
using NClient.Common.Helpers;
using NClient.Core.Helpers.EqualityComparers;
using NClient.Invocation;
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
            return new NClientFactoryResilienceSetter<TRequest, TResponse>(
                _builderContextModifier, 
                methodPredicate: (method, _) => method.Info.DeclaringType == typeof(TClient));
        }
        
        public INClientFactoryResilienceSetter<TRequest, TResponse> ForMethodOf<TClient>(Expression<Func<TClient, Delegate>> selector)
        {
            Ensure.IsNotNull(selector, nameof(selector));
            
            var func = selector.Compile();
            var selectedMethod = func.Invoke(default!).Method;
            return new NClientFactoryResilienceSetter<TRequest, TResponse>(
                _builderContextModifier, 
                methodPredicate: (method, _) => _methodInfoEqualityComparer.Equals(method.Info, selectedMethod));
        }
        
        public INClientFactoryResilienceSetter<TRequest, TResponse> ForMethodsThat(Func<IMethod, IRequest, bool> condition)
        {
            Ensure.IsNotNull(condition, nameof(condition));
            
            return new NClientFactoryResilienceSetter<TRequest, TResponse>(
                _builderContextModifier, 
                condition);
        }
    }
}
