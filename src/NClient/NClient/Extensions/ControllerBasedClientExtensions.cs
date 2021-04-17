using System;
using System.Net.Http;
using NClient.Abstractions.HttpClients;
using NClient.ControllerBasedClients;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using Polly;

namespace NClient.Extensions
{
    public static class ControllerBasedClientExtensions
    {
        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(
            this INClientBuilder clientBuilder, string host)
            where TInterface : class
            where TController : TInterface
        {
            return clientBuilder.Use<TInterface, TController>(host, new SystemHttpClientProvider());
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(
            this INClientBuilder clientBuilder, string host, IHttpClientFactory httpClientFactory)
            where TInterface : class
            where TController : TInterface
        {
            return clientBuilder.Use<TInterface, TController>(host, new SystemHttpClientProvider(httpClientFactory));
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(
            this INClientBuilder clientBuilder, string host, HttpMessageHandler httpMessageHandler)
            where TInterface : class
            where TController : TInterface
        {
            return clientBuilder.Use<TInterface, TController>(host, new SystemHttpClientProvider(httpMessageHandler));
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use WithResiliencePolicy<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> WithResiliencePolicy<TInterface, TController>(
            this IControllerBasedClientBuilder<TInterface, TController> clientBuilder, IAsyncPolicy<HttpResponse> asyncPolicy)
            where TInterface : class
            where TController : TInterface
        {
            return clientBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
