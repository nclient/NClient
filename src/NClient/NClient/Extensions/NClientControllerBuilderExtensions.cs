using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.Resilience.Polly;
using NClient.Standalone;
using Polly;
using RestSharp.Authenticators;

#pragma warning disable 618

namespace NClient.Extensions
{
    public static class NClientControllerBuilderExtensions
    {
        public static INClientControllerBuilder<TInterface, TController> Use<TInterface, TController>(
            this INClientControllerBuilder clientBuilder, string host)
            where TInterface : class
            where TController : TInterface
        {
            return clientBuilder.Use<TInterface, TController>(host, new RestSharpHttpClientProvider());
        }

        public static INClientControllerBuilder<TInterface, TController> Use<TInterface, TController>(
            this INClientControllerBuilder clientBuilder, string host, IAuthenticator authenticator)
            where TInterface : class
            where TController : TInterface
        {
            return clientBuilder.Use<TInterface, TController>(host, new RestSharpHttpClientProvider(authenticator));
        }

        public static INClientControllerBuilder<TInterface, TController> WithResiliencePolicy<TInterface, TController>(
            this INClientControllerBuilder<TInterface, TController> clientBuilder, IAsyncPolicy asyncPolicy)
            where TInterface : class
            where TController : TInterface
        {
            return clientBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
