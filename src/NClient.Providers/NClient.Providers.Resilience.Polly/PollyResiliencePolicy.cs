using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    internal class PollyResiliencePolicy : IResiliencePolicy
    {
        private readonly IAsyncPolicy<ResponseContext> _asyncPolicy;

        public PollyResiliencePolicy(
            IAsyncPolicy<ResponseContext> asyncPolicy)
        {
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            _asyncPolicy = asyncPolicy;
        }

        public async Task<HttpResponse> ExecuteAsync(
            Func<Task<ResponseContext>> action)
        {
            Ensure.IsNotNull(action, nameof(action));

            var policyResult = await _asyncPolicy.ExecuteAndCaptureAsync(action).ConfigureAwait(false);
            return policyResult.Result.HttpResponse;
        }
    }
}
