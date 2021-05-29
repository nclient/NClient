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
    public static class InterfaceBasedClientExtensions
    {
        public static IOptionalNClientBuilder<TInterface> WithCustomHttpClient<TInterface>(
            this IOptionalNClientBuilder<TInterface> clientBuilder,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return clientBuilder.WithCustomHttpClient(new SystemHttpClientProvider(httpClientFactory, httpClientName));
        }

        public static IOptionalNClientBuilder<TInterface> WithCustomHttpClient<TInterface>(
            this IOptionalNClientBuilder<TInterface> clientBuilder,
            HttpMessageHandler httpMessageHandler)
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(httpMessageHandler, nameof(httpMessageHandler));

            return clientBuilder.WithCustomHttpClient(new SystemHttpClientProvider(httpMessageHandler));
        }

        public static IOptionalNClientBuilder<TInterface> WithCustomSerializer<TInterface>(
            this IOptionalNClientBuilder<TInterface> clientBuilder,
            JsonSerializerOptions jsonSerializerOptions)
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return clientBuilder.WithCustomSerializer(new SystemSerializerProvider(jsonSerializerOptions));
        }

        public static IOptionalNClientBuilder<TInterface> WithResiliencePolicy<TInterface>(
            this IOptionalNClientBuilder<TInterface> clientBuilder,
            IAsyncPolicy<HttpResponse> asyncPolicy)
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            return clientBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
