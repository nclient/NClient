using NClient.Abstractions.Resilience;

namespace NClient.Core.Resilience
{
    internal class StubResiliencePolicyProvider<TRequest, TResponse> : IResiliencePolicyProvider<TRequest, TResponse>
    {
        public IResiliencePolicy<TRequest, TResponse> Create()
        {
            return new StubResiliencePolicy<TRequest, TResponse>();
        }
    }
}
