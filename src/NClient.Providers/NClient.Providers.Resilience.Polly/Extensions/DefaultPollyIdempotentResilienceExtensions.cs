using System;
using NClient.Common.Helpers;
using NClient.Core.Extensions;
using NClient.Providers.Resilience;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class DefaultPollyIdempotentResilienceExtensions
    {
        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="clientAdvancedOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithPollyIdempotentResilience<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(settings, nameof(settings));
            
            return clientAdvancedOptionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(new ResiliencePolicySettings<TRequest, TResponse>(
                        maxRetries: 0,
                        getDelay: _ => TimeSpan.FromSeconds(0),
                        shouldRetry: settings.ShouldRetry)))
                    .ForMethodsThat((_, request) => request.Type.IsIdempotent())
                    .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings)));
        }
        
        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithPollyIdempotentResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(settings, nameof(settings));
            
            return WithPollyIdempotentResilience(clientOptionalBuilder.AsAdvanced(), settings).AsBasic();
        }
        
        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithPollyIdempotentResilience<TRequest, TResponse>(
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
                .ForMethodsThat((_, request) => request.Type.IsIdempotent())
                .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings)));
        }
        
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithPollyIdempotentResilience<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder,
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(getDelay, nameof(getDelay));
            Ensure.IsNotNull(shouldRetry, nameof(shouldRetry));
            
            return clientAdvancedOptionalBuilder.WithPollyIdempotentResilience(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
        
        // TODO: doc
        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithPollyIdempotentResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(getDelay, nameof(getDelay));
            Ensure.IsNotNull(shouldRetry, nameof(shouldRetry));
            
            return WithPollyIdempotentResilience(clientOptionalBuilder.AsAdvanced(), maxRetries, getDelay, shouldRetry).AsBasic();
        }
        
        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithPollyIdempotentResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            
            return factoryOptionalBuilder.WithPollyIdempotentResilience(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
    }
}
