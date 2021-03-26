using Microsoft.AspNetCore.Mvc;
using NClient.Abstractions.Clients;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.Resilience.Polly;
using RestSharp.Authenticators;
using Polly;

namespace NClient.AspNetProxy.Extensions
{
    public static class ClientProviderExtensions
    {
        public static IControllerClientProvider<TInterface, TController> Use<TInterface, TController>(
            this IControllerClientProvider clientProvider, string host)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return clientProvider.Use<TInterface, TController>(host, new RestSharpHttpClientProvider());
        }

        public static IControllerClientProvider<TInterface, TController> Use<TInterface, TController>(
            this IControllerClientProvider clientProvider, string host, IAuthenticator authenticator)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return clientProvider.Use<TInterface, TController>(host, new RestSharpHttpClientProvider(authenticator));
        }

        public static IControllerClientProvider<TInterface, TController> WithPollyResiliencePolicy<TInterface, TController>(
            this IControllerClientProvider<TInterface, TController> clientProvider, IAsyncPolicy asyncPolicy)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return clientProvider.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
