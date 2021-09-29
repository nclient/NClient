using System.Net.Http;
using NClient.Abstractions.Resilience;

namespace NClient.Resilience
{
    internal class DefaultResiliencePolicyProvider : IResiliencePolicyProvider<HttpRequestMessage, HttpResponseMessage>
    {
        public IResiliencePolicy<HttpRequestMessage, HttpResponseMessage> Create()
        {
            return new DefaultResiliencePolicy();
        }
    }
}
