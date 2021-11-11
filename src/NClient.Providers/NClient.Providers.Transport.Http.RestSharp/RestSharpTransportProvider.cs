using NClient.Common.Helpers;
using RestSharp;
using RestSharp.Authenticators;

namespace NClient.Providers.Transport.Http.RestSharp
{
    /// <summary>
    /// The RestSharp based provider for a component that can create <see cref="ITransport{TRequest,TResponse}"/> instances.
    /// </summary>
    public class RestSharpTransportProvider : ITransportProvider<IRestRequest, IRestResponse>
    {
        private readonly IAuthenticator? _authenticator;

        /// <summary>
        /// Creates the RestSharp based HTTP client provider.
        /// </summary>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public RestSharpTransportProvider(IAuthenticator? authenticator = null)
        {
            _authenticator = authenticator;
        }

        public ITransport<IRestRequest, IRestResponse> Create(IToolSet toolSet)
        {
            Ensure.IsNotNull(toolSet, nameof(toolSet));

            return new RestSharpTransport(_authenticator);
        }
    }
}
