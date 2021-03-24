using Microsoft.AspNetCore.Mvc;
using NClient.Core;
using NClient.Providers.HttpClient.RestSharp;

namespace NClient.AspNetProxy.Extensions
{
    public static class ClientProviderHttpExtensions
    {
        public static IControllerClientProviderResilience<TInterface, TController> SetDefaultHttpClientProvider<TInterface, TController>(
            this IControllerClientProviderHttp<TInterface, TController>  clientProvider)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return clientProvider.SetHttpClientProvider(new RestSharpHttpClientProvider());
        }
    }
}
