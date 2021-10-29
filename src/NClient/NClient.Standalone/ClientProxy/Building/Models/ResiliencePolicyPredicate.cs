using System;
using NClient.Invocation;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Building.Models
{
    internal class ResiliencePolicyPredicate<TRequest, TResponse>
    {
        public IResiliencePolicyProvider<TRequest, TResponse> Provider { get; }
        public Func<IMethod, IRequest, bool> Predicate { get; }
        
        public ResiliencePolicyPredicate(IResiliencePolicyProvider<TRequest, TResponse> provider, Func<IMethod, IRequest, bool> predicate)
        {
            Provider = provider;
            Predicate = predicate;
        }
    }
}
