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
    internal class NClientFactoryResilienceSetter<TRequest, TResponse> : INClientFactoryResilienceSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        private readonly IEnumerable<Func<IMethod, IRequest, bool>> _methodPredicates;
        
        public NClientFactoryResilienceSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier, Func<IMethod, IRequest, bool> methodPredicate) 
            : this(builderContextModifier, new[] { methodPredicate })
        {
        }
        
        public NClientFactoryResilienceSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier, IEnumerable<Func<IMethod, IRequest, bool>> methodPredicates)
        {
            _builderContextModifier = builderContextModifier;
            _methodPredicates = methodPredicates;
        }

        public INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicy<TRequest, TResponse> policy)
        {
            Ensure.IsNotNull(policy, nameof(policy));
            
            return Use(new ResiliencePolicyProvider<TRequest, TResponse>(policy));
        }
        
        public INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            Ensure.IsNotNull(provider, nameof(provider));
            
            _builderContextModifier.Add(context => context.WithResiliencePolicy(_methodPredicates, provider));
            return new NClientFactoryResilienceMethodSelector<TRequest, TResponse>(_builderContextModifier);
        }
        
        public INClientFactoryResilienceMethodSelector<TRequest, TResponse> DoNotUse()
        {
            _builderContextModifier.Add(context => context.WithResiliencePolicy(_methodPredicates, new StubResiliencePolicyProvider<TRequest, TResponse>()));
            return new NClientFactoryResilienceMethodSelector<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
