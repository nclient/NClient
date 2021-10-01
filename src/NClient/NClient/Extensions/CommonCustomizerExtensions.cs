using System.Net.Http;
using NClient.Abstractions.Customization;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Serialization.System;
using NClient.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class CommonCustomizerExtensions
    {
        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider{TRequest,TResponse}"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        public static TCustomizer UsingHttpClient<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage> commonCustomizer)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));

            return commonCustomizer.UsingSystemHttpClient();
        }
        
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        public static TCustomizer UsingSerializer<TCustomizer, TInterface, TRequest, TResponse>(
            this INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse> commonCustomizer)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));

            return commonCustomizer.UsingSystemJsonSerializer();
        }
        
        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static TCustomizer WithForceResilience<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage> commonCustomizer,
            IResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>? settings = null)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));

            settings ??= new DefaultResiliencePolicySettings();
            return commonCustomizer.WithForcePollyResilience(settings);
        }

        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static TCustomizer WithSafeResilience<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage> commonCustomizer,
            IResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>? settings = null)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));

            settings ??= new DefaultResiliencePolicySettings();
            return commonCustomizer.WithSafePollyResilience(settings);
        }

        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="settings">The settings for default resilience policy provider.</param>
        public static TCustomizer WithIdempotentResilience<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage> commonCustomizer,
            IResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>? settings = null)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));

            settings ??= new DefaultResiliencePolicySettings();
            return commonCustomizer.WithIdempotentPollyResilience(settings);
        }
    }
}
