using System;
using System.Collections.Generic;
using System.Reflection;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;
using NClient.Standalone.Client.Resilience;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Resilience
{
    internal class NClientFactoryResilienceSetter<TRequest, TResponse> : INClientFactoryResilienceSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        private readonly IEnumerable<Func<MethodInfo, IRequest, bool>> _methodPredicates;
        
        public NClientFactoryResilienceSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier, Func<MethodInfo, IRequest, bool> methodPredicate) 
            : this(builderContextModifier, new[] { methodPredicate })
        {
        }
        
        public NClientFactoryResilienceSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier, IEnumerable<Func<MethodInfo, IRequest, bool>> methodPredicates)
        {
            _builderContextModifier = builderContextModifier;
            _methodPredicates = methodPredicates;
        }

        public INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider)
        {
            _builderContextModifier.Add(context => context.WithResiliencePolicy(_methodPredicates, resiliencePolicyProvider));
            return new NClientFactoryResilienceMethodSelector<TRequest, TResponse>(_builderContextModifier);
        }
        
        public INClientFactoryResilienceMethodSelector<TRequest, TResponse> DoNotUse()
        {
            _builderContextModifier.Add(context => context.WithResiliencePolicy(_methodPredicates, new StubResiliencePolicyProvider<TRequest, TResponse>()));
            return new NClientFactoryResilienceMethodSelector<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
