using NClient.Abstractions.Providers.HttpClient;
using NClient.Abstractions.Providers.Serialization;
using NClient.Common.Helpers;
using RestSharp;
using RestSharp.Authenticators;

namespace NClient.Providers.HttpClient.RestSharp
{
    /// <summary>
    /// The RestSharp based provider for a component that can create <see cref="IHttpClient{TRequest,TResponse}"/> instances.
    /// </summary>
    public class RestSharpHttpClientProvider : IHttpClientProvider<IRestRequest, IRestResponse>
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

        public IHttpClient<IRestRequest, IRestResponse> Create(ISerializer serializer)
        {
            Ensure.IsNotNull(serializer, nameof(serializer));

            return new RestSharpHttpClient(_authenticator);
        }
    }
}
