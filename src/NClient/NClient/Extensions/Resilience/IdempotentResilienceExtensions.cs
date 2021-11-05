using System;
using System.Net.Http;
using NClient.Common.Helpers;
using NClient.Providers.Transport;
using NClient.Providers.Transport.Http.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class IdempotentResilienceExtensions
    {
        // TODO: doc
        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="clientAdvancedOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithIdempotentResilience<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> clientAdvancedOptionalBuilder,
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));

            return clientAdvancedOptionalBuilder.WithPollyIdempotentResilience(
                new DefaultSystemResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }

        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="clientAdvancedOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientFactoryAdvancedOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithIdempotentResilience(
            this INClientFactoryAdvancedOptionalBuilder<HttpRequestMessage, HttpResponseMessage> clientAdvancedOptionalBuilder,
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));

            return clientAdvancedOptionalBuilder.WithPollyIdempotentResilience(
                new DefaultSystemResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }
        
        // TODO: doc
        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithIdempotentResilience(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> clientOptionalBuilder,
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));

            return WithIdempotentResilience(clientOptionalBuilder.AsAdvanced(), maxRetries, getDelay, shouldRetry).AsBasic();
        }
    }
}
