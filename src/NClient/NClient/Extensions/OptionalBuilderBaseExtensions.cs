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
using Polly;

namespace NClient.Extensions
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
    }
}
