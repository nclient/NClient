using System.Net.Http;
using NClient.Abstractions.Builders;
using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using NClient.Providers.Resilience.Polly;
using NClient.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ForceResilienceExtensions
    {
        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithForceResilience<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> clientOptionalBuilder,
            IResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>? settings = null)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));

            settings ??= new DefaultResiliencePolicySettings();
            return clientOptionalBuilder.WithForcePollyResilience(settings);
        }
        
        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithForceResilience(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder,
            IResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>? settings = null)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));

            settings ??= new DefaultResiliencePolicySettings();
            return factoryOptionalBuilder.WithForcePollyResilience(settings);
        }
    }
}
