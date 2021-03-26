using NClient.Abstractions.Resilience;

namespace NClient.Core.Resilience
{
    internal class StubResiliencePolicyProvider : IResiliencePolicyProvider
    {
        public IResiliencePolicy Create()
        {
            return new StubResiliencePolicy();
        }
    }
}
