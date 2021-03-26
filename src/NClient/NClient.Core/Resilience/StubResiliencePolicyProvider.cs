using NClient.Abstractions.Resilience;

namespace NClient.Core.Resilience
{
    public class StubResiliencePolicyProvider : IResiliencePolicyProvider
    {
        public IResiliencePolicy Create()
        {
            return new StubResiliencePolicy();
        }
    }
}
