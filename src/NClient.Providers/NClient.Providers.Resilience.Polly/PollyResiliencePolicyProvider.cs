using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    public class PollyResiliencePolicyProvider : IResiliencePolicyProvider
    {
        private readonly IAsyncPolicy<HttpResponse> _asyncPolicy;

        public PollyResiliencePolicyProvider(IAsyncPolicy<HttpResponse> asyncPolicy)
        {
            _asyncPolicy = asyncPolicy;
        }

        public IResiliencePolicy Create()
        {
            return new PollyResiliencePolicy(_asyncPolicy);
        }
    }
}
