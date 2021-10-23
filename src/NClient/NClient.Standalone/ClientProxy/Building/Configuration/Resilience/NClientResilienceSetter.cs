using System;
using System.Collections.Generic;
using System.Reflection;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;
using NClient.Standalone.Client.Resilience;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Resilience
{
    internal class NClientResilienceSetter<TClient, TRequest, TResponse> : INClientResilienceSetter<TClient, TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        private readonly IEnumerable<Func<MethodInfo, IHttpRequest, bool>> _methodPredicates;
        
        public NClientResilienceSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier, Func<MethodInfo, IHttpRequest, bool> methodPredicate) 
            : this(builderContextModifier, new[] { methodPredicate })
        {
        }
        
        public NClientResilienceSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier, IEnumerable<Func<MethodInfo, IHttpRequest, bool>> methodPredicates)
        {
            _builderContextModifier = builderContextModifier;
            _methodPredicates = methodPredicates;
        }

        INClientResilienceMethodSelector<TClient, TRequest, TResponse> INClientResilienceSetter<TClient, TRequest, TResponse>.Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider)
        {
            _builderContextModifier.Add(context => context.WithResiliencePolicy(_methodPredicates, resiliencePolicyProvider));
            return new NClientResilienceMethodSelector<TClient, TRequest, TResponse>(_builderContextModifier);
        }
        
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> INClientResilienceSetter<TClient, TRequest, TResponse>.DoNotUse()
        {
            _builderContextModifier.Add(context => context.WithResiliencePolicy(_methodPredicates, new StubResiliencePolicyProvider<TRequest, TResponse>()));
            return new NClientResilienceMethodSelector<TClient, TRequest, TResponse>(_builderContextModifier);
        }
    }
}
