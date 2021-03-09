using NClient.Providers.Resilience.Abstractions;

namespace NClient.Providers.Resilience
{
    public class StubResiliencePolicyProvider : IResiliencePolicyProvider
    {
        public IResiliencePolicy Create()
        {
            return new StubResiliencePolicy();
        }
    }
}
