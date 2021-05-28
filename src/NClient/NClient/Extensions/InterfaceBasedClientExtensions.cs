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
        public static IInterfaceBasedClientBuilder<TInterface> WithCustomHttpClient<TInterface>(
            this IInterfaceBasedClientBuilder<TInterface> clientBuilder,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return clientBuilder.WithCustomHttpClient(new SystemHttpClientProvider(httpClientFactory, httpClientName));
        }

        public static IInterfaceBasedClientBuilder<TInterface> WithCustomHttpClient<TInterface>(
            this IInterfaceBasedClientBuilder<TInterface> clientBuilder,
            HttpMessageHandler httpMessageHandler)
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(httpMessageHandler, nameof(httpMessageHandler));

            return clientBuilder.WithCustomHttpClient(new SystemHttpClientProvider(httpMessageHandler));
        }

        public static IInterfaceBasedClientBuilder<TInterface> WithCustomSerializer<TInterface>(
            this IInterfaceBasedClientBuilder<TInterface> clientBuilder,
            JsonSerializerOptions jsonSerializerOptions)
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return clientBuilder.WithCustomSerializer(new SystemSerializerProvider(jsonSerializerOptions));
        }

        public static IInterfaceBasedClientBuilder<TInterface> WithResiliencePolicy<TInterface>(
            this IInterfaceBasedClientBuilder<TInterface> clientBuilder,
            IAsyncPolicy<HttpResponse> asyncPolicy)
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            return clientBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
