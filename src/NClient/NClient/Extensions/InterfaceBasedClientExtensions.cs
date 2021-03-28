using NClient.InterfaceBasedClients;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.Resilience.Polly;
using Polly;
using RestSharp.Authenticators;

namespace NClient.Extensions
{
    public static class InterfaceBasedClientExtensions
    {
        public static IInterfaceBasedClientBuilder<T> Use<T>(this INClientBuilder clientBuilder, string host) where T : class
        {
            return clientBuilder.Use<T>(host, new RestSharpHttpClientProvider());
        }

        public static IInterfaceBasedClientBuilder<T> Use<T>(
            this INClientBuilder clientBuilder, string host, IAuthenticator authenticator) where T : class
        {
            return clientBuilder.Use<T>(host, new RestSharpHttpClientProvider(authenticator));
        }

        public static IInterfaceBasedClientBuilder<T> WithResiliencePolicy<T>(
            this IInterfaceBasedClientBuilder<T> clientBuilder, IAsyncPolicy asyncPolicy) where T : class
        {
            return clientBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
