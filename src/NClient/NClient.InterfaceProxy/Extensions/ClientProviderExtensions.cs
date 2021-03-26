using NClient.Abstractions.Clients;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.Resilience.Polly;
using RestSharp.Authenticators;
using Polly;

namespace NClient.InterfaceProxy.Extensions
{
    public static class ClientProviderExtensions
    {
        public static INClientProvider<T> Use<T>(this INClientProvider clientProvider, string host) where T : class, INClient
        {
            return clientProvider.Use<T>(host, new RestSharpHttpClientProvider());
        }

        public static INClientProvider<T> Use<T>(
            this INClientProvider clientProvider, string host, IAuthenticator authenticator) where T : class, INClient
        {
            return clientProvider.Use<T>(host, new RestSharpHttpClientProvider(authenticator));
        }

        public static INClientProvider<T> WithResiliencePolicy<T>(
            this INClientProvider<T> clientProvider, IAsyncPolicy asyncPolicy) where T : class, INClient
        {
            return clientProvider.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
