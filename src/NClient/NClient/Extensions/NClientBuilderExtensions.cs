using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.Resilience.Polly;
using NClient.Standalone;
using Polly;
using RestSharp.Authenticators;

namespace NClient.Extensions
{
    public static class NClientBuilderExtensions
    {
        public static INClientBuilder<T> Use<T>(this INClientBuilder clientBuilder, string host) where T : class
        {
            return clientBuilder.Use<T>(host, new RestSharpHttpClientProvider());
        }

        public static INClientBuilder<T> Use<T>(
            this INClientBuilder clientBuilder, string host, IAuthenticator authenticator) where T : class
        {
            return clientBuilder.Use<T>(host, new RestSharpHttpClientProvider(authenticator));
        }

        public static INClientBuilder<T> WithResiliencePolicy<T>(
            this INClientBuilder<T> clientBuilder, IAsyncPolicy asyncPolicy) where T : class
        {
            return clientBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
