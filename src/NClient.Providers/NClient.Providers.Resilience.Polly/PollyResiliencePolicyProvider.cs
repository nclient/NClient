using NClient.Providers.Resilience.Abstractions;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    public class PollyResiliencePolicyProvider : IResiliencePolicyProvider
    {
        private readonly IAsyncPolicy _asyncPolicy;

        public PollyResiliencePolicyProvider(IAsyncPolicy asyncPolicy)
        {
            _asyncPolicy = asyncPolicy;
        }

        public IResiliencePolicy Create()
        {
            return new PollyResiliencePolicy(_asyncPolicy);
        }
    }
}
