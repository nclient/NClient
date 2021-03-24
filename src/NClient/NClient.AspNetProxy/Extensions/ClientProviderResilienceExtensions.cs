using Microsoft.AspNetCore.Mvc;
using NClient.Core;
using NClient.Providers.Resilience.Polly;
using Polly;

namespace NClient.AspNetProxy.Extensions
{
    public static class ClientProviderResilienceExtensions
    {
        public static IControllerClientProviderLogger<TInterface, TController> WithPollyResiliencePolicy<TInterface, TController>(
            this IControllerClientProviderResilience<TInterface, TController> clientProvider, IAsyncPolicy asyncPolicy)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return clientProvider.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
