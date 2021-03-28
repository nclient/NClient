using NClient.Abstractions.HttpClients;

namespace NClient.Standalone
{
    public static class NClientStandaloneProvider
    {
        public static INClientBuilder<T> Use<T>(string host, IHttpClientProvider httpClientProvider) where T : class
        {
            return new NClientBuilder().Use<T>(host, httpClientProvider);
        }
    }
}
