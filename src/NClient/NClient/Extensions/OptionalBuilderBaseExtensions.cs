using System.Net.Http;
using System.Text.Json;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Common.Helpers;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Serialization.System;
using Polly;

namespace NClient.Extensions
{
    public static class OptionalBuilderBaseExtensions
    {
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

        public static TBuilder WithResiliencePolicy<TBuilder, TInterface>(
            this IOptionalBuilderBase<TBuilder, TInterface> clientBuilder,
            IAsyncPolicy<HttpResponse> asyncPolicy)
            where TBuilder : IOptionalBuilderBase<TBuilder, TInterface>
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            return clientBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
