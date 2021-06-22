using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    /// <summary>
    /// The Polly based resilience policy.
    /// </summary>
    public class PollyResiliencePolicy : IResiliencePolicy
    {
        private readonly IAsyncPolicy<ResponseContext> _asyncPolicy;

        /// <summary>
        /// Creates the Polly based resilience policy.
        /// </summary>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
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

            var responseContext = await _asyncPolicy.ExecuteAsync(action).ConfigureAwait(false);
            return responseContext.HttpResponse;
        }
    }
}
