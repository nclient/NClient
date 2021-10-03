using System;
using NClient.Abstractions.Builders;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Resilience.Settings;
using NClient.Common.Helpers;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Resilience.Polly
{
    public static class CommonCustomizerExtensions
    {
        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithIdempotentPollyResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            
            return clientOptionalBuilder.WithIdempotentResilience(
                idempotentMethodProvider: new ConfiguredPollyResiliencePolicyProvider<TRequest, TResponse>(settings), 
                otherMethodProvider: new ConfiguredPollyResiliencePolicyProvider<TRequest, TResponse>(new ResiliencePolicySettings<TRequest, TResponse>
                (
                    retryCount: 0,
                    sleepDuration: _ => TimeSpan.FromSeconds(0),
                    resultPredicate: settings.ResultPredicate,
                    onFallbackAsync: settings.OnFallbackAsync
                )));
        }
        
        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithIdempotentPollyResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            
            return factoryOptionalBuilder.WithIdempotentResilience(
                idempotentMethodProvider: new ConfiguredPollyResiliencePolicyProvider<TRequest, TResponse>(settings), 
                otherMethodProvider: new ConfiguredPollyResiliencePolicyProvider<TRequest, TResponse>(new ResiliencePolicySettings<TRequest, TResponse>
                (
                    retryCount: 0,
                    sleepDuration: _ => TimeSpan.FromSeconds(0),
                    resultPredicate: settings.ResultPredicate,
                    onFallbackAsync: settings.OnFallbackAsync
                )));
        }
        
        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="idempotentMethodPolicy">The settings for resilience policy provider for idempotent methods.</param>
        /// <param name="otherMethodPolicy">The settings for resilience policy provider for other methods.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithIdempotentPollyResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> idempotentMethodPolicy, IAsyncPolicy<ResponseContext<TRequest, TResponse>> otherMethodPolicy)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            
            return clientOptionalBuilder.WithIdempotentResilience(
                idempotentMethodProvider: new PollyResiliencePolicyProvider<TRequest, TResponse>(idempotentMethodPolicy), 
                otherMethodProvider: new PollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy));
        }
        
        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="idempotentMethodPolicy">The settings for resilience policy provider for idempotent methods.</param>
        /// <param name="otherMethodPolicy">The settings for resilience policy provider for other methods.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithIdempotentPollyResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> idempotentMethodPolicy, IAsyncPolicy<ResponseContext<TRequest, TResponse>> otherMethodPolicy)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            
            return factoryOptionalBuilder.WithIdempotentResilience(
                idempotentMethodProvider: new PollyResiliencePolicyProvider<TRequest, TResponse>(idempotentMethodPolicy), 
                otherMethodProvider: new PollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy));
        }
    }
}
