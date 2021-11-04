using System;
using System.Net.Http;
using NClient.Common.Helpers;
using NClient.Providers.Transport;
using NClient.Providers.Transport.Http.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SafeResilienceExtensions
    {
        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="clientAdvancedOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientAdvancedOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithSafeResilience<TClient>(
            this INClientAdvancedOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> clientAdvancedOptionalBuilder,
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));

            return clientAdvancedOptionalBuilder.WithSafePollyResilience(
                new DefaultSystemResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }
        
        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithSafeResilience<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> clientOptionalBuilder,
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));

            return WithSafeResilience(clientOptionalBuilder.AsAdvanced(), maxRetries, getDelay, shouldRetry).AsBasic();
        }
        
        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithSafeResilience(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder,
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));

            return factoryOptionalBuilder.WithSafePollyResilience(
                new DefaultSystemResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }
    }
}
