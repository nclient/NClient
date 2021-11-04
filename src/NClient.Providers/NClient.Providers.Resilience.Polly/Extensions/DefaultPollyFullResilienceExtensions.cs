using System;
using NClient.Common.Helpers;
using NClient.Providers.Resilience;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class DefaultPollyFullResilienceExtensions
    {
        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithPollyFullResilience<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(settings, nameof(settings));
            
            return clientOptionalBuilder.WithResilience(x => x
                .ForAllMethods()
                .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings)));
        }
        
        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithPollyFullResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(settings, nameof(settings));
            
            return WithPollyFullResilience(clientOptionalBuilder.AsAdvanced(), settings).AsBasic();
        }
        
        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithPollyFullResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            
            return factoryOptionalBuilder.WithCustomResilience(x => x
                .ForAllMethods()
                .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings)));
        }
        
        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="clientAdvancedOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithPollyFullResilience<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder,
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(getDelay, nameof(getDelay));
            Ensure.IsNotNull(shouldRetry, nameof(shouldRetry));
            
            return clientAdvancedOptionalBuilder.WithPollyFullResilience(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
        
        // TODO: doc
        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithPollyFullResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(getDelay, nameof(getDelay));
            Ensure.IsNotNull(shouldRetry, nameof(shouldRetry));
            
            return WithPollyFullResilience(
                    clientOptionalBuilder.AsAdvanced(), maxRetries, getDelay, shouldRetry)
                .AsBasic();
        }
        
        // TODO: doc
        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithPollyFullResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            
            return factoryOptionalBuilder.WithPollyFullResilience(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
    }
}
