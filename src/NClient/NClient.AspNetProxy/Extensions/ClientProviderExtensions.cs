using Microsoft.AspNetCore.Mvc;
using NClient.Abstractions.Clients;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.Resilience.Polly;
using RestSharp.Authenticators;
using Polly;
#pragma warning disable 618

namespace NClient.AspNetProxy.Extensions
{
    public static class ClientProviderExtensions
    {
        public static INClientControllerProvider<TInterface, TController> Use<TInterface, TController>(
            this INClientControllerProvider clientProvider, string host)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return clientProvider.Use<TInterface, TController>(host, new RestSharpHttpClientProvider());
        }

        public static INClientControllerProvider<TInterface, TController> Use<TInterface, TController>(
            this INClientControllerProvider clientProvider, string host, IAuthenticator authenticator)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return clientProvider.Use<TInterface, TController>(host, new RestSharpHttpClientProvider(authenticator));
        }

        public static INClientControllerProvider<TInterface, TController> WithPollyResiliencePolicy<TInterface, TController>(
            this INClientControllerProvider<TInterface, TController> clientProvider, IAsyncPolicy asyncPolicy)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return clientProvider.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
