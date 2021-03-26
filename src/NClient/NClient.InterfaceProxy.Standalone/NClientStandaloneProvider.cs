using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;

namespace NClient.InterfaceProxy
{
    public static class NClientStandaloneProvider
    {
        public static INClientBuilder<T> Use<T>(string host, IHttpClientProvider httpClientProvider) where T : class, INClient
        {
            return new NClientBuilder().Use<T>(host, httpClientProvider);
        }
    }
}
