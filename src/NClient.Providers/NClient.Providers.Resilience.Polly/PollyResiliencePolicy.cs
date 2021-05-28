using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    public class PollyResiliencePolicy : IResiliencePolicy
    {
        private readonly IAsyncPolicy<HttpResponse> _asyncPolicy;

        public PollyResiliencePolicy(IAsyncPolicy<HttpResponse> asyncPolicy)
        {
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            _asyncPolicy = asyncPolicy;
        }
        public Task<HttpResponse> ExecuteAsync(Func<Task<HttpResponse>> action)
        {
            Ensure.IsNotNull(action, nameof(action));

            return _asyncPolicy.ExecuteAsync(action);
        }
    }
}
