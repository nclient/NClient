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
    public class PollyResiliencePolicy<TRequest, TResponse> : IResiliencePolicy<TRequest, TResponse>
    {
        private readonly IAsyncPolicy<IResponseContext<TRequest, TResponse>> _asyncPolicy;

        /// <summary>
        /// Creates the Polly based resilience policy.
        /// </summary>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public PollyResiliencePolicy(IAsyncPolicy<IResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            _asyncPolicy = asyncPolicy;
        }

        public async Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(Func<Task<IResponseContext<TRequest, TResponse>>> action)
        {
            Ensure.IsNotNull(action, nameof(action));

            var responseContext = await _asyncPolicy.ExecuteAsync(action).ConfigureAwait(false);
            return responseContext;
        }
    }
}
