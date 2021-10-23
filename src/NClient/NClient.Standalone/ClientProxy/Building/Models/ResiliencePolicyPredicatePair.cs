using System;
using System.Reflection;
using NClient.Abstractions.Providers.Resilience;
using NClient.Abstractions.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Building.Models
{
    internal class ResiliencePolicyPredicatePair<TRequest, TResponse>
    {
        public IResiliencePolicyProvider<TRequest, TResponse> Provider { get; }
        public Func<MethodInfo, IHttpRequest, bool> Predicate { get; }
        
        public ResiliencePolicyPredicatePair(IResiliencePolicyProvider<TRequest, TResponse> provider, Func<MethodInfo, IHttpRequest, bool> predicate)
        {
            Provider = provider;
            Predicate = predicate;
        }
    }
}
