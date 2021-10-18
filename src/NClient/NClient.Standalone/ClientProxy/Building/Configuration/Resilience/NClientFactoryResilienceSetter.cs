using System;
using System.Collections.Generic;
using System.Reflection;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Standalone.Client.Resilience;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Resilience
{
    internal class NClientFactoryResilienceSetter<TRequest, TResponse> : INClientFactoryResilienceSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        private readonly IEnumerable<Func<MethodInfo, IHttpRequest, bool>> _methodPredicates;
        
        public NClientFactoryResilienceSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier, Func<MethodInfo, IHttpRequest, bool> methodPredicate) 
            : this(builderContextModifier, new[] { methodPredicate })
        {
        }
        
        public NClientFactoryResilienceSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier, IEnumerable<Func<MethodInfo, IHttpRequest, bool>> methodPredicates)
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
