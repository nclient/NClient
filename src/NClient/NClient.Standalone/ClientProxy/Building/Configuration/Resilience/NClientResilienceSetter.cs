using System;
using System.Collections.Generic;
using NClient.Common.Helpers;
using NClient.Invocation;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;
using NClient.Standalone.Client.Resilience;
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

        public INClientResilienceMethodSelector<TClient, TRequest, TResponse> Use(IResiliencePolicy<TRequest, TResponse> policy)
        {
            Ensure.IsNotNull(policy, nameof(policy));
            
            return Use(new ResiliencePolicyProvider<TRequest, TResponse>(policy));
        }
        
        public INClientResilienceMethodSelector<TClient, TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            Ensure.IsNotNull(provider, nameof(provider));
            
            _builderContextModifier.Add(context => context.WithResiliencePolicy(_methodPredicates, provider));
            return new NClientResilienceMethodSelector<TClient, TRequest, TResponse>(_builderContextModifier);
        }
        
        public INClientResilienceMethodSelector<TClient, TRequest, TResponse> DoNotUse()
        {
            _builderContextModifier.Add(context => context.WithResiliencePolicy(_methodPredicates, new StubResiliencePolicyProvider<TRequest, TResponse>()));
            return new NClientResilienceMethodSelector<TClient, TRequest, TResponse>(_builderContextModifier);
        }
    }
}
