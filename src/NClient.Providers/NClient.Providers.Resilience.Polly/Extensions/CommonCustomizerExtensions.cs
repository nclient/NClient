using System;
using NClient.Abstractions.Customization;
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
        /// Sets Polly based <see cref="IResiliencePolicyProvider"/> used to create instance of <see cref="IResiliencePolicy"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public static TCustomizer WithPollyResilience<TCustomizer, TInterface, TRequest, TResponse>(
            this INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse> commonCustomizer,
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> asyncPolicy)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            return commonCustomizer.WithForceResilience(new PollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy));
        }

        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static TCustomizer WithForcePollyResilience<TCustomizer, TInterface, TRequest, TResponse>(
            this INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse> commonCustomizer,
            IResiliencePolicySettings<TRequest, TResponse> settings)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));
            
            return commonCustomizer.WithForceResilience(new ConfiguredPollyResiliencePolicyProvider<TRequest, TResponse>(settings));
        }

        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static TCustomizer WithSafePollyResilience<TCustomizer, TInterface, TRequest, TResponse>(
            this INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse> commonCustomizer,
            IResiliencePolicySettings<TRequest, TResponse> settings)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));
            
            return commonCustomizer.WithSafeResilience(
                safeMethodProvider: new ConfiguredPollyResiliencePolicyProvider<TRequest, TResponse>(settings), 
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
        /// <param name="commonCustomizer"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static TCustomizer WithIdempotentPollyResilience<TCustomizer, TInterface, TRequest, TResponse>(
            this INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse> commonCustomizer,
            IResiliencePolicySettings<TRequest, TResponse> settings)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));
            
            return commonCustomizer.WithIdempotentResilience(
                idempotentMethodProvider: new ConfiguredPollyResiliencePolicyProvider<TRequest, TResponse>(settings), 
                otherMethodProvider: new ConfiguredPollyResiliencePolicyProvider<TRequest, TResponse>(new ResiliencePolicySettings<TRequest, TResponse>
                (
                    retryCount: 0,
                    sleepDuration: _ => TimeSpan.FromSeconds(0),
                    resultPredicate: settings.ResultPredicate,
                    onFallbackAsync: settings.OnFallbackAsync
                )));
        }
    }
}
