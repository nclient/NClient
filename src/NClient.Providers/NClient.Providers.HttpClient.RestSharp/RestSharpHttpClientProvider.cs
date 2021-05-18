using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
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

        public IHttpClient Create(ISerializer serializer)
        {
            return new RestSharpHttpClient(_authenticator);
        }
    }
}
