using System.Net.Http;
using NClient.Abstractions.Resilience;

namespace NClient.Resilience
{
    internal class DefaultResiliencePolicyProvider : IResiliencePolicyProvider<HttpResponseMessage>
    {
        public IResiliencePolicy<HttpResponseMessage> Create()
        {
            return new DefaultResiliencePolicy();
        }
    }
}
