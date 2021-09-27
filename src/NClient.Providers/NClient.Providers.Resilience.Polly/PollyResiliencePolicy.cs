using System;
using System.Threading.Tasks;
using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    /// <summary>
    /// The Polly based resilience policy.
    /// </summary>
    public class PollyResiliencePolicy<TResponse> : IResiliencePolicy<TResponse>
    {
        private readonly IAsyncPolicy<ResponseContext<TResponse>> _asyncPolicy;

        /// <summary>
        /// Creates the Polly based resilience policy.
        /// </summary>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public PollyResiliencePolicy(IAsyncPolicy<ResponseContext<TResponse>> asyncPolicy)
        {
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            _asyncPolicy = asyncPolicy;
        }

        public async Task<TResponse> ExecuteAsync(Func<Task<ResponseContext<TResponse>>> action)
        {
            Ensure.IsNotNull(action, nameof(action));

            var responseContext = await _asyncPolicy.ExecuteAsync(action).ConfigureAwait(false);
            return responseContext.Response;
        }
    }
}
