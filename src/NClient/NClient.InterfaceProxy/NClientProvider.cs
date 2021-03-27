using NClient.Providers.HttpClient.RestSharp;
using RestSharp.Authenticators;

namespace NClient.InterfaceProxy
{
    public static class NClientProvider
    {
        public static INClientBuilder<T> Use<T>(string host) where T : class
        {
            return new NClientBuilder().Use<T>(host, new RestSharpHttpClientProvider());
        }

        public static INClientBuilder<T> Use<T>(string host, IAuthenticator authenticator) where T : class
        {
            return new NClientBuilder().Use<T>(host, new RestSharpHttpClientProvider(authenticator));
        }
    }
}
