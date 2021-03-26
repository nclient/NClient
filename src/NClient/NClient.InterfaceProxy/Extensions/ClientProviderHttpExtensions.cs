using NClient.Abstractions.Clients;
using NClient.Core;
using NClient.Providers.HttpClient.RestSharp;
using RestSharp.Authenticators;

namespace NClient.InterfaceProxy.Extensions
{
    public static class ClientProviderHttpExtensions
    {
        public static IClientProviderResilience<T> SetDefaultHttpClientProvider<T>(
            this IClientProviderHttp<T>  clientProvider) where T : class, INClient
        {
            return clientProvider.SetHttpClientProvider(new RestSharpHttpClientProvider());
        }

        public static IClientProviderResilience<T> SetDefaultHttpClientProvider<T>(
            this IClientProviderHttp<T> clientProvider, IAuthenticator authenticator) where T : class, INClient
        {
            return clientProvider.SetHttpClientProvider(new RestSharpHttpClientProvider(authenticator));
        }
    }
}
