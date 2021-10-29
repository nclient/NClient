using System;
using System.Collections.Generic;
using NClient.Invocation;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Building.Context;
using NClient.Standalone.ClientProxy.Validation.Resilience;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Resilience
{
    internal class NClientResilienceSetter<TClient, TRequest, TResponse> : INClientResilienceSetter<TClient, TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        private readonly IEnumerable<Func<IMethod, IRequest, bool>> _methodPredicates;
        
        public NClientResilienceSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier, Func<IMethod, IRequest, bool> methodPredicate) 
            : this(builderContextModifier, new[] { methodPredicate })
        {
        }
        
        public NClientResilienceSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier, IEnumerable<Func<IMethod, IRequest, bool>> methodPredicates)
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
