using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Resilience
{
    internal class DefaultResiliencePolicyProvider : IResiliencePolicyProvider<HttpResponse>
    {
        public IResiliencePolicy<HttpResponse> Create()
        {
            return new DefaultResiliencePolicy();
        }
    }
}
