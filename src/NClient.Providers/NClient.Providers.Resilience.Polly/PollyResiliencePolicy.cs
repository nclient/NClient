using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;
using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    internal class PollyResiliencePolicy : IResiliencePolicy
    {
        private readonly IAsyncPolicy<(HttpResponse Response, MethodInvocation MethodInvocation)> _asyncPolicy;

        public PollyResiliencePolicy(
            IAsyncPolicy<(HttpResponse Response, MethodInvocation MethodInvocation)> asyncPolicy)
        {
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            _asyncPolicy = asyncPolicy;
        }

        public async Task<HttpResponse> ExecuteAsync(
            Func<Task<(HttpResponse Response, MethodInvocation MethodInvocation)>> action)
        {
            Ensure.IsNotNull(action, nameof(action));

            var policyResult = await _asyncPolicy.ExecuteAndCaptureAsync(action).ConfigureAwait(false);
            return policyResult.Result.Response;
        }
    }
}
