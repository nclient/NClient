using NClient.Abstractions.HttpClients;
using RestSharp.Authenticators;

namespace NClient.Providers.HttpClient.RestSharp
{
    public class RestSharpHttpClientProvider : IHttpClientProvider
    {
        private readonly IAuthenticator? _authenticator;

        public RestSharpHttpClientProvider(IAuthenticator? authenticator = null)
        {
            _authenticator = authenticator;
        }

        public IHttpClient Create()
        {
            return new RestSharpHttpClient(_authenticator);
        }
    }
}
