using System;
using System.Reflection;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Building.Models
{
    internal class ResiliencePolicyPredicatePair<TRequest, TResponse>
    {
        public IResiliencePolicyProvider<TRequest, TResponse> Provider { get; }
        public Func<MethodInfo, IRequest, bool> Predicate { get; }
        
        public ResiliencePolicyPredicatePair(IResiliencePolicyProvider<TRequest, TResponse> provider, Func<MethodInfo, IRequest, bool> predicate)
        {
            Provider = provider;
            Predicate = predicate;
        }
    }
}
