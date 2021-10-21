using NClient.Abstractions.Providers.Resilience;

namespace NClient.Standalone.Client.Resilience
{
    internal class StubResiliencePolicyProvider<TRequest, TResponse> : IResiliencePolicyProvider<TRequest, TResponse>
    {
        public IResiliencePolicy<TRequest, TResponse> Create()
        {
            return new StubResiliencePolicy<TRequest, TResponse>();
        }
    }
}
