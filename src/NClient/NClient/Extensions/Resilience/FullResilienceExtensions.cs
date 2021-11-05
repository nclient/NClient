using System;
using System.Net.Http;
using NClient.Common.Helpers;
using NClient.Providers.Transport;
using NClient.Providers.Transport.Http.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class FullResilienceExtensions
    {
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithFullResilience<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> clientAdvancedOptionalBuilder,
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            
            return clientAdvancedOptionalBuilder.WithPollyFullResilience(
                new DefaultSystemResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }

        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithFullResilience(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> clientAdvancedOptionalBuilder,
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            
            return clientAdvancedOptionalBuilder.WithPollyFullResilience(
                new DefaultSystemResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }
    }
}
