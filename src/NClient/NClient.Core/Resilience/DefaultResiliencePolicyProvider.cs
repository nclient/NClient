using NClient.Abstractions.Resilience;

namespace NClient.Core.Resilience
{
    internal class DefaultResiliencePolicyProvider<TResponse> : IResiliencePolicyProvider<TResponse>
    {
        public IResiliencePolicy<TResponse> Create()
        {
            return new DefaultResiliencePolicy<TResponse>();
        }
    }
}
