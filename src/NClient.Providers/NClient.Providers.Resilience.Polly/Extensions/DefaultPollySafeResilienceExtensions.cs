using System;
using NClient.Common.Helpers;
using NClient.Core.Extensions;
using NClient.Providers.Resilience;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class DefaultPollySafeResilienceExtensions
    {
        /// <summary>Sets a resilience policy for safe methods (Info/Options, Check/Head, Read/Get).</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="settings">The settings for resilience policy provider.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithPollySafeResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(settings, nameof(settings));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(new ResiliencePolicySettings<TRequest, TResponse>(
                        maxRetries: 0,
                        getDelay: _ => TimeSpan.FromSeconds(0), 
                        shouldRetry: settings.ShouldRetry)))
                    .ForMethodsThat((_, request) => request.Type.IsSafe())
                    .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings)));
        }

        /// <summary>Sets a resilience policy for safe methods (Info/Options, Check/Head, Read/Get).</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithPollySafeResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(getDelay, nameof(getDelay));
            Ensure.IsNotNull(shouldRetry, nameof(shouldRetry));
            
            return optionalBuilder.WithPollySafeResilience(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }

        /// <summary>Sets a resilience policy for safe methods (Info/Options, Check/Head, Read/Get).</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="settings">The settings for resilience policy provider.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithPollySafeResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(settings, nameof(settings));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(new ResiliencePolicySettings<TRequest, TResponse>(
                        maxRetries: 0,
                        getDelay: _ => TimeSpan.FromSeconds(0), 
                        shouldRetry: settings.ShouldRetry)))
                    .ForMethodsThat((_, request) => request.Type.IsSafe())
                    .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings)));
        }

        /// <summary>Sets a resilience policy for safe methods (Info/Options, Check/Head, Read/Get).</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithPollySafeResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(getDelay, nameof(getDelay));
            Ensure.IsNotNull(shouldRetry, nameof(shouldRetry));
            
            return optionalBuilder.WithPollySafeResilience(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
    }
}
