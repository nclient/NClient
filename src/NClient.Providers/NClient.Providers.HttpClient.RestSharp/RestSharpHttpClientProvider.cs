using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Providers.HttpClient.RestSharp.Builders;
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
            Ensure.IsNotNull(serializer, nameof(serializer));

            var httpRequestMessageBuilder = new RestRequestBuilder();
            var httpResponseBuilder = new HttpResponseBuilder();
            var httpResponsePopulater = new HttpResponsePopulater(serializer);

            return new RestSharpHttpClient(
                httpRequestMessageBuilder,
                httpResponseBuilder,
                httpResponsePopulater,
                _authenticator);
        }
    }
}
