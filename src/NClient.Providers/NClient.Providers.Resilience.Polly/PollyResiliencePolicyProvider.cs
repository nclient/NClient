using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    /// <summary>
    /// The Polly based provider for a component that can create <see cref="IResiliencePolicy"/> instances.
    /// </summary>
    public class PollyResiliencePolicyProvider<TRequest, TResponse> : IResiliencePolicyProvider<TRequest, TResponse>
    {
        private readonly IAsyncPolicy<ResponseContext<TRequest, TResponse>> _asyncPolicy;

        /// <summary>
        /// Creates the Polly based resilience policy provider.
        /// </summary>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public PollyResiliencePolicyProvider(
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            _asyncPolicy = asyncPolicy;
        }

        public IResiliencePolicy<TRequest, TResponse> Create()
        {
            return new PollyResiliencePolicy<TRequest, TResponse>(_asyncPolicy);
        }
    }
}
