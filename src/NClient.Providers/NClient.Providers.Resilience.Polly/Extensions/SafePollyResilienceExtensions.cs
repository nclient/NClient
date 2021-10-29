using System;
using NClient.Common.Helpers;
using NClient.Core.Extensions;
using NClient.Providers.Resilience;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Transport;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SafePollyResilienceExtensions
    {
        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSafePollyResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            
            return clientOptionalBuilder.WithCustomResilience(x => x
                .ForAllMethods()
                .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(new ResiliencePolicySettings<TRequest, TResponse>(
                    maxRetries: 0,
                    getDelay: _ => TimeSpan.FromSeconds(0), 
                    shouldRetry: settings.ShouldRetry)))
                .ForMethodsThat((_, request) =>
                {
                    return request.Type.IsSafe();
                })
                .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings)));
        }
        
        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSafePollyResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            
            return factoryOptionalBuilder.WithCustomResilience(x => x
                .ForAllMethods()
                .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(new ResiliencePolicySettings<TRequest, TResponse>(
                    maxRetries: 0,
                    getDelay: _ => TimeSpan.FromSeconds(0), 
                    shouldRetry: settings.ShouldRetry)))
                .ForMethodsThat((_, request) => request.Type.IsSafe())
                .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings)));
        }
        
        // TODO: doc
        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSafePollyResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            
            return clientOptionalBuilder.WithSafePollyResilience(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
        
        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSafePollyResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            
            return factoryOptionalBuilder.WithSafePollyResilience(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
        
        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="safeMethodPolicy">The settings for resilience policy provider for safe methods.</param>
        /// <param name="otherMethodPolicy">The settings for resilience policy provider for other methods.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSafePollyResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> safeMethodPolicy, IAsyncPolicy<IResponseContext<TRequest, TResponse>> otherMethodPolicy)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            
            return clientOptionalBuilder.WithCustomResilience(x => x
                .ForAllMethods()
                .Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy))
                .ForMethodsThat((_, request) => request.Type.IsSafe())
                .Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(safeMethodPolicy)));
        }
        
        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="safeMethodPolicy">The settings for resilience policy provider for safe methods.</param>
        /// <param name="otherMethodPolicy">The settings for resilience policy provider for other methods.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSafePollyResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> safeMethodPolicy, IAsyncPolicy<IResponseContext<TRequest, TResponse>> otherMethodPolicy)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            
            return factoryOptionalBuilder.WithCustomResilience(x => x
                .ForAllMethods()
                .Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy))
                .ForMethodsThat((_, request) => request.Type.IsSafe())
                .Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(safeMethodPolicy)));
        }
    }
}
