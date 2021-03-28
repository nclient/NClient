using System;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.Resilience.Polly;
using NClient.Standalone;
using NClient.Standalone.ControllerBasedClients;
using Polly;
using RestSharp.Authenticators;

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
            return clientBuilder.Use<TInterface, TController>(host, new RestSharpHttpClientProvider());
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(
            this INClientBuilder clientBuilder, string host, IAuthenticator authenticator)
            where TInterface : class
            where TController : TInterface
        {
            return clientBuilder.Use<TInterface, TController>(host, new RestSharpHttpClientProvider(authenticator));
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use WithResiliencePolicy<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> WithResiliencePolicy<TInterface, TController>(
            this IControllerBasedClientBuilder<TInterface, TController> clientBuilder, IAsyncPolicy asyncPolicy)
            where TInterface : class
            where TController : TInterface
        {
            return clientBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
