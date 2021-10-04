using System;
using NClient.Abstractions.Builders;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Resilience.Settings;
using NClient.Common.Helpers;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Resilience.Polly
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
            
            return clientOptionalBuilder.WithSafeResilience(
                safeMethodProvider: new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings), 
                otherMethodProvider: new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(new ResiliencePolicySettings<TRequest, TResponse>
                (
                    retryCount: 0,
                    sleepDuration: _ => TimeSpan.FromSeconds(0),
                    resultPredicate: settings.ResultPredicate
                )));
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
            
            return factoryOptionalBuilder.WithSafeResilience(
                safeMethodProvider: new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings), 
                otherMethodProvider: new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(new ResiliencePolicySettings<TRequest, TResponse>
                (
                    retryCount: 0,
                    sleepDuration: _ => TimeSpan.FromSeconds(0),
                    resultPredicate: settings.ResultPredicate
                )));
        }
        
        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="safeMethodPolicy">The settings for resilience policy provider for safe methods.</param>
        /// <param name="otherMethodPolicy">The settings for resilience policy provider for other methods.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSafePollyResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> safeMethodPolicy, IAsyncPolicy<ResponseContext<TRequest, TResponse>> otherMethodPolicy)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            
            return clientOptionalBuilder.WithSafeResilience(
                safeMethodProvider: new PollyResiliencePolicyProvider<TRequest, TResponse>(safeMethodPolicy), 
                otherMethodProvider: new PollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy));
        }
        
        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="safeMethodPolicy">The settings for resilience policy provider for safe methods.</param>
        /// <param name="otherMethodPolicy">The settings for resilience policy provider for other methods.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSafePollyResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> safeMethodPolicy, IAsyncPolicy<ResponseContext<TRequest, TResponse>> otherMethodPolicy)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            
            return factoryOptionalBuilder.WithSafeResilience(
                safeMethodProvider: new PollyResiliencePolicyProvider<TRequest, TResponse>(safeMethodPolicy), 
                otherMethodProvider: new PollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy));
        }
    }
}
