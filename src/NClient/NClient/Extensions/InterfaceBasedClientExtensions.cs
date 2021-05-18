using System.Net.Http;
using System.Text.Json;
using NClient.Abstractions.HttpClients;
using NClient.InterfaceBasedClients;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Serialization.System;
using Polly;

namespace NClient.Extensions
{
    public static class InterfaceBasedClientExtensions
    {
        public static IInterfaceBasedClientBuilder<T> Use<T>(this INClientBuilder clientBuilder, string host) where T : class
        {
            return clientBuilder.Use<T>(host, new SystemHttpClientProvider(), new SystemSerializerProvider());
        }

        public static IInterfaceBasedClientBuilder<T> Use<T>(
            this INClientBuilder clientBuilder, string host, IHttpClientFactory httpClientFactory, string? httpClientName = null) where T : class
        {
            return clientBuilder.Use<T>(host, new SystemHttpClientProvider(httpClientFactory, httpClientName), new SystemSerializerProvider());
        }

        public static IInterfaceBasedClientBuilder<T> Use<T>(
            this INClientBuilder clientBuilder, string host, HttpMessageHandler httpMessageHandler) where T : class
        {
            return clientBuilder.Use<T>(host, new SystemHttpClientProvider(httpMessageHandler), new SystemSerializerProvider());
        }

        public static IInterfaceBasedClientBuilder<T> SetJsonSerializerOptions<T>(
            this IInterfaceBasedClientBuilder<T> clientBuilder, JsonSerializerOptions jsonSerializerOptions) where T : class
        {
            return clientBuilder.SetCustomSerializer(new SystemSerializerProvider(jsonSerializerOptions));
        }

        public static IInterfaceBasedClientBuilder<T> WithResiliencePolicy<T>(
            this IInterfaceBasedClientBuilder<T> clientBuilder, IAsyncPolicy<HttpResponse> asyncPolicy) where T : class
        {
            return clientBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
