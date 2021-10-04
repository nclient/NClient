using NClient.Abstractions.Builders;
using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Resilience.Polly
{
    public static class ForcePollyResilienceExtensions
    {
        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithForcePollyResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            
            return clientOptionalBuilder.WithForceResilience(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings));
        }
        
        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithForcePollyResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            
            return factoryOptionalBuilder.WithForceResilience(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings));
        }

        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithForcePollyResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> asyncPolicy)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            
            return clientOptionalBuilder.WithForceResilience(new PollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy));
        }
        
        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithForcePollyResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            
            return factoryOptionalBuilder.WithForceResilience(new PollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy));
        }
    }
}
