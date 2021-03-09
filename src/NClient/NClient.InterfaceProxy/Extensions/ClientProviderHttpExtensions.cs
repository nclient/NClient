using NClient.Core;
using NClient.Providers.HttpClient.RestSharp;

namespace NClient.InterfaceProxy.Extensions
{
    public static class ClientProviderHttpExtensions
    {
        public static IClientProviderResilience<T> SetDefaultHttpClientProvider<T>(
            this IClientProviderHttp<T>  clientProvider) where T : class, INClient
        {
            return clientProvider.SetHttpClientProvider(new RestSharpHttpClientProvider());
        }
    }
}
