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
    public static class OptionalBuilderBaseExtensions
    {
        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="clientBuilder"></param>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="System.Net.Http.HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="System.Net.Http.HttpClient"/> to create.</param>
        public static TBuilder WithCustomHttpClient<TBuilder, TInterface>(
            this IOptionalBuilderBase<TBuilder, TInterface> clientBuilder,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
            where TBuilder : IOptionalBuilderBase<TBuilder, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return clientBuilder.WithCustomHttpClient(new SystemHttpClientProvider(httpClientFactory, httpClientName));
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="clientBuilder"></param>
        /// <param name="httpMessageHandler">The HTTP message handler.</param>
        public static TBuilder WithCustomHttpClient<TBuilder, TInterface>(
            this IOptionalBuilderBase<TBuilder, TInterface> clientBuilder,
            HttpMessageHandler httpMessageHandler)
            where TBuilder : IOptionalBuilderBase<TBuilder, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(httpMessageHandler, nameof(httpMessageHandler));

            return clientBuilder.WithCustomHttpClient(new SystemHttpClientProvider(httpMessageHandler));
        }

        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientBuilder"></param>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public static TBuilder WithCustomSerializer<TBuilder, TInterface>(
            this IOptionalBuilderBase<TBuilder, TInterface> clientBuilder,
            JsonSerializerOptions jsonSerializerOptions)
            where TBuilder : IOptionalBuilderBase<TBuilder, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return clientBuilder.WithCustomSerializer(new SystemSerializerProvider(jsonSerializerOptions));
        }

        /// <summary>
        /// Sets Polly based <see cref="IResiliencePolicyProvider"/> used to create instance of <see cref="IResiliencePolicy"/>.
        /// </summary>
        /// <param name="clientBuilder"></param>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public static TBuilder WithResiliencePolicy<TBuilder, TInterface>(
            this IOptionalBuilderBase<TBuilder, TInterface> clientBuilder,
            IAsyncPolicy<ResponseContext> asyncPolicy)
            where TBuilder : IOptionalBuilderBase<TBuilder, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            return clientBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }

        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="clientBuilder"></param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="resultPredicate">The predicate to filter the results this policy will handle.</param>
        public static TBuilder WithResiliencePolicy<TBuilder, TInterface>(
            this IOptionalBuilderBase<TBuilder, TInterface> clientBuilder,
            int retryCount = 2,
            Func<int, TimeSpan>? sleepDurationProvider = null,
            Func<ResponseContext, bool>? resultPredicate = null)
            where TBuilder : IOptionalBuilderBase<TBuilder, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));

            return clientBuilder.WithResiliencePolicy(new AnyMethodResiliencePolicyProvider(retryCount, sleepDurationProvider, resultPredicate));
        }

        /// <summary>
        /// Sets resilience policy provider for safe HTTP methods (GET, HEAD, OPTIONS).
        /// </summary>
        /// <param name="clientBuilder"></param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="resultPredicate">The predicate to filter the results this policy will handle.</param>
        public static TBuilder WithResiliencePolicyForSafeMethods<TBuilder, TInterface>(
            this IOptionalBuilderBase<TBuilder, TInterface> clientBuilder,
            int retryCount = 2,
            Func<int, TimeSpan>? sleepDurationProvider = null,
            Func<ResponseContext, bool>? resultPredicate = null)
            where TBuilder : IOptionalBuilderBase<TBuilder, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));

            return clientBuilder.WithResiliencePolicy(new SafeMethodResiliencePolicyProvider(retryCount, sleepDurationProvider, resultPredicate));
        }

        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="clientBuilder"></param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="resultPredicate">The predicate to filter the results this policy will handle.</param>
        public static TBuilder WithResiliencePolicyForIdempotentMethods<TBuilder, TInterface>(
            this IOptionalBuilderBase<TBuilder, TInterface> clientBuilder,
            int retryCount = 2,
            Func<int, TimeSpan>? sleepDurationProvider = null,
            Func<ResponseContext, bool>? resultPredicate = null)
            where TBuilder : IOptionalBuilderBase<TBuilder, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));

            return clientBuilder.WithResiliencePolicy(new IdempotentMethodResiliencePolicyProvider(retryCount, sleepDurationProvider, resultPredicate));
        }
    }
}
