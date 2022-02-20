using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Common.Helpers;
using NClient.Providers.Transport;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    /// <summary>The Polly based resilience policy.</summary>
    public class PollyResiliencePolicy<TRequest, TResponse> : IResiliencePolicy<TRequest, TResponse>
    {
        private readonly IAsyncPolicy<IResponseContext<TRequest, TResponse>> _asyncPolicy;

        /// <summary>Creates the Polly based resilience policy.</summary>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public PollyResiliencePolicy(IAsyncPolicy<IResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            _asyncPolicy = asyncPolicy;
        }

        /// <summary>Executes the specified asynchronous action within the policy and returns the result.</summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">The cancellation token to terminate the policy.</param>
        public async Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(
            Func<CancellationToken, Task<IResponseContext<TRequest, TResponse>>> action, CancellationToken cancellationToken)
        {
            Ensure.IsNotNull(action, nameof(action));
            cancellationToken.ThrowIfCancellationRequested();

            var responseContext = await _asyncPolicy.ExecuteAsync(action, cancellationToken).ConfigureAwait(false);
            return responseContext;
        }
    }
}
