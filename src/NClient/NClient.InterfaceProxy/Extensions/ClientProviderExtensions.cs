using NClient.Abstractions.Clients;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.Resilience.Polly;
using RestSharp.Authenticators;
using Polly;

namespace NClient.InterfaceProxy.Extensions
{
    public static class ClientProviderExtensions
    {
        public static IClientProvider<T> Use<T>(this IClientProvider clientProvider, string host) where T : class, INClient
        {
            return clientProvider.Use<T>(host, new RestSharpHttpClientProvider());
        }

        public static IClientProvider<T> Use<T>(
            this IClientProvider clientProvider, string host, IAuthenticator authenticator) where T : class, INClient
        {
            return clientProvider.Use<T>(host, new RestSharpHttpClientProvider(authenticator));
        }

        public static IClientProvider<T> WithResiliencePolicy<T>(
            this IClientProvider<T> clientProvider, IAsyncPolicy asyncPolicy) where T : class, INClient
        {
            return clientProvider.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
