using System.Net.Http;
using NClient.Abstractions.HttpClients;
using NClient.InterfaceBasedClients;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using Polly;

namespace NClient.Extensions
{
    public static class InterfaceBasedClientExtensions
    {
        public static IInterfaceBasedClientBuilder<T> Use<T>(this INClientBuilder clientBuilder, string host) where T : class
        {
            return clientBuilder.Use<T>(host, new SystemHttpClientProvider());
        }

        public static IInterfaceBasedClientBuilder<T> Use<T>(
            this INClientBuilder clientBuilder, string host, IHttpClientFactory httpClientFactory, string? httpClientName = null) where T : class
        {
            return clientBuilder.Use<T>(host, new SystemHttpClientProvider(httpClientFactory, httpClientName));
        }

        public static IInterfaceBasedClientBuilder<T> Use<T>(
            this INClientBuilder clientBuilder, string host, HttpMessageHandler httpMessageHandler) where T : class
        {
            return clientBuilder.Use<T>(host, new SystemHttpClientProvider(httpMessageHandler));
        }

        public static IInterfaceBasedClientBuilder<T> WithResiliencePolicy<T>(
            this IInterfaceBasedClientBuilder<T> clientBuilder, IAsyncPolicy<HttpResponse> asyncPolicy) where T : class
        {
            return clientBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
