using System;
using NClient.Providers.Resilience;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class DefaultUsePollyExtensions
    {
        /// <summary>Sets a resilience policy for specific method/methods.</summary>
        /// <param name="clientResilienceSetter"></param>
        /// <param name="settings">The settings for resilience policy provider.</param>
        public static INClientResilienceMethodSelector<TClient, TRequest, TResponse> UsePolly<TClient, TRequest, TResponse>(
            this INClientResilienceSetter<TClient, TRequest, TResponse> clientResilienceSetter, 
            IResiliencePolicySettings<TRequest, TResponse> settings)
        {
            return clientResilienceSetter.Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings));
        }
        
        /// <summary>Sets a resilience policy for specific method/methods.</summary>
        /// <param name="factoryResilienceSetter"></param>
        /// <param name="settings">The settings for resilience policy provider.</param>
        public static INClientFactoryResilienceMethodSelector<TRequest, TResponse> UsePolly<TRequest, TResponse>(
            this INClientFactoryResilienceSetter<TRequest, TResponse> factoryResilienceSetter, 
            IResiliencePolicySettings<TRequest, TResponse> settings)
        {
            return factoryResilienceSetter.Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(settings));
        }
        
        /// <summary>Sets a resilience policy for specific method/methods.</summary>
        /// <param name="clientResilienceSetter"></param>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
        public static INClientResilienceMethodSelector<TClient, TRequest, TResponse> UsePolly<TClient, TRequest, TResponse>(
            this INClientResilienceSetter<TClient, TRequest, TResponse> clientResilienceSetter, 
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
        {
            return clientResilienceSetter.UsePolly(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
        
        /// <summary>Sets a resilience policy for specific method/methods.</summary>
        /// <param name="factoryResilienceSetter"></param>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
        public static INClientFactoryResilienceMethodSelector<TRequest, TResponse> UsePolly<TRequest, TResponse>(
            this INClientFactoryResilienceSetter<TRequest, TResponse> factoryResilienceSetter, 
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
        {
            return factoryResilienceSetter.UsePolly(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
    }
}
