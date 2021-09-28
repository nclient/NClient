using System;
using System.Net.Http;
using System.Text.Json;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Serialization.System;
using NClient.Resilience;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class CommonCustomizerExtensions
    {
        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="System.Net.Http.HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="System.Net.Http.HttpClient"/> to create.</param>
        public static TCustomizer WithCustomHttpClient<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface> commonCustomizer,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return commonCustomizer.WithCustomHttpClient(new SystemHttpClientProvider(httpClientFactory, httpClientName));
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="httpMessageHandler">The HTTP message handler.</param>
        public static TCustomizer WithCustomHttpClient<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface> commonCustomizer,
            HttpMessageHandler httpMessageHandler)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));
            Ensure.IsNotNull(httpMessageHandler, nameof(httpMessageHandler));

            return commonCustomizer.WithCustomHttpClient(new SystemHttpClientProvider(httpMessageHandler));
        }

        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public static TCustomizer WithCustomSerializer<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface> commonCustomizer,
            JsonSerializerOptions jsonSerializerOptions)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return commonCustomizer.WithCustomSerializer(new SystemSerializerProvider(jsonSerializerOptions));
        }

        /// <summary>
        /// Sets Polly based <see cref="IResiliencePolicyProvider"/> used to create instance of <see cref="IResiliencePolicy"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public static TCustomizer WithResiliencePolicy<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface> commonCustomizer,
            IAsyncPolicy<ResponseContext> asyncPolicy)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            return commonCustomizer.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }

        /// <summary>
        /// Sets resilience policy provider for all HTTP methods.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="resultPredicate">The predicate to filter the results this policy will handle.</param>
        public static TCustomizer WithResiliencePolicy<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface> commonCustomizer,
            int retryCount = 2,
            Func<int, TimeSpan>? sleepDurationProvider = null,
            Func<ResponseContext, bool>? resultPredicate = null)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));

            return commonCustomizer.WithResiliencePolicy(new AnyMethodResiliencePolicyProvider(retryCount, sleepDurationProvider, resultPredicate));
        }

        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="resultPredicate">The predicate to filter the results this policy will handle.</param>
        public static TCustomizer WithResiliencePolicyForSafeMethods<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface> commonCustomizer,
            int retryCount = 2,
            Func<int, TimeSpan>? sleepDurationProvider = null,
            Func<ResponseContext, bool>? resultPredicate = null)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));

            return commonCustomizer.WithResiliencePolicy(new SafeMethodResiliencePolicyProvider(retryCount, sleepDurationProvider, resultPredicate));
        }

        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="resultPredicate">The predicate to filter the results this policy will handle.</param>
        public static TCustomizer WithResiliencePolicyForIdempotentMethods<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface> commonCustomizer,
            int retryCount = 2,
            Func<int, TimeSpan>? sleepDurationProvider = null,
            Func<ResponseContext, bool>? resultPredicate = null)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));

            return commonCustomizer.WithResiliencePolicy(new IdempotentMethodResiliencePolicyProvider(retryCount, sleepDurationProvider, resultPredicate));
        }
    }
}
