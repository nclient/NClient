using NClient.Abstractions.Exceptions.Factories;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Providers.HttpClient.RestSharp.Builders;
using RestSharp.Authenticators;

namespace NClient.Providers.HttpClient.RestSharp
{
    /// <summary>
    /// The RestSharp based provider for a component that can create <see cref="IHttpClient"/> instances.
    /// </summary>
    public class RestSharpHttpClientProvider : IHttpClientProvider
    {
        private readonly IAuthenticator? _authenticator;

        /// <summary>
        /// Creates the RestSharp based HTTP client provider.
        /// </summary>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public RestSharpHttpClientProvider(IAuthenticator? authenticator = null)
        {
            _authenticator = authenticator;
        }

        public IHttpClient Create(ISerializer serializer)
        {
            Ensure.IsNotNull(serializer, nameof(serializer));

            var httpRequestMessageBuilder = new RestRequestBuilder(serializer);
            var httpResponseBuilder = new HttpResponseBuilder(new ClientHttpRequestExceptionFactory());

            return new RestSharpHttpClient(
                httpRequestMessageBuilder,
                httpResponseBuilder,
                _authenticator);
        }
    }
}
