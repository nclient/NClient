using NClient.Abstractions.Resilience;

namespace NClient.Core.Resilience
{
    internal class DefaultResiliencePolicyProvider : IResiliencePolicyProvider
    {
        public IResiliencePolicy Create()
        {
            return new DefaultResiliencePolicy();
        }
    }
}
