using NClient.Providers.Resilience;

namespace NClient.Standalone.ClientProxy.Validation.Resilience
{
    internal class StubResiliencePolicyProvider<TRequest, TResponse> : IResiliencePolicyProvider<TRequest, TResponse>
    {
        public IResiliencePolicy<TRequest, TResponse> Create()
        {
            return new StubResiliencePolicy<TRequest, TResponse>();
        }
    }
}
