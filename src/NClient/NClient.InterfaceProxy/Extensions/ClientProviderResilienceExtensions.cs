using NClient.Abstractions.Clients;
using NClient.Core;
using NClient.Providers.Resilience.Polly;
using Polly;

namespace NClient.InterfaceProxy.Extensions
{
    public static class ClientProviderResilienceExtensions
    {
        public static IClientProviderLogger<T> WithPollyResiliencePolicy<T>(
            this IClientProviderResilience<T> clientProvider, IAsyncPolicy asyncPolicy) where T : class, INClient
        {
            return clientProvider.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
