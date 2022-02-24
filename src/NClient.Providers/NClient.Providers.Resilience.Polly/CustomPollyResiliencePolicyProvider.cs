using NClient.Common.Helpers;
using NClient.Providers.Transport;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    /// <summary>The custom Polly based provider for a component that can create <see cref="IResiliencePolicy"/> instances.</summary>
    public class CustomPollyResiliencePolicyProvider<TRequest, TResponse> : IResiliencePolicyProvider<TRequest, TResponse>
    {
        private readonly IAsyncPolicy<IResponseContext<TRequest, TResponse>> _asyncPolicy;

        /// <summary>Initializes the custom Polly based resilience policy provider.</summary>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public CustomPollyResiliencePolicyProvider(
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            _asyncPolicy = asyncPolicy;
        }

        /// <summary>Creates the custom Polly based resilience policy.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        public IResiliencePolicy<TRequest, TResponse> Create(IToolset toolset)
        {
            return new PollyResiliencePolicy<TRequest, TResponse>(_asyncPolicy);
        }
    }
}
