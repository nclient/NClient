using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    public class PollyResiliencePolicy : IResiliencePolicy
    {
        private readonly IAsyncPolicy<HttpResponse> _asyncPolicy;

        public PollyResiliencePolicy(IAsyncPolicy<HttpResponse> asyncPolicy)
        {
            _asyncPolicy = asyncPolicy;
        }
        public Task<HttpResponse> ExecuteAsync(Func<Task<HttpResponse>> action, string policyKey)
        {
            return _asyncPolicy.WithPolicyKey(policyKey).ExecuteAsync(action);
        }
    }
}
