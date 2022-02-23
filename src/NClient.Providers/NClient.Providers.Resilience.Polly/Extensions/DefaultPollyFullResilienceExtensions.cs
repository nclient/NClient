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
        /// <summary>Sets a resilience policy for all types of methods.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithPollyFullResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(settings, nameof(settings));
            
            return optionalBuilder.WithResilience(x => x
                .ForAllMethods()
                .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings)));
        }

        /// <summary>Sets a resilience policy for all types of methods.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithPollyFullResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(getDelay, nameof(getDelay));
            Ensure.IsNotNull(shouldRetry, nameof(shouldRetry));
            
            return optionalBuilder.WithPollyFullResilience(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }

        /// <summary>Sets a resilience policy for all types of methods..</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="settings">The settings for resilience policy provider.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithPollyFullResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            IResiliencePolicySettings<TRequest, TResponse> settings)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(settings, nameof(settings));
            
            return optionalBuilder.WithResilience(x => x
                .ForAllMethods()
                .Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings)));
        }

        /// <summary>Sets a resilience policy for all types of methods.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithPollyFullResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(getDelay, nameof(getDelay));
            Ensure.IsNotNull(shouldRetry, nameof(shouldRetry));
            
            return optionalBuilder.WithPollyFullResilience(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
    }
}
