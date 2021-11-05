using NClient.Common.Helpers;
using NClient.Providers.Transport;
using NClient.Providers.Transport.Http.RestSharp;
using RestSharp;
using RestSharp.Authenticators;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class RestSharpTransportExtensions
    {
        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientAdvancedTransportBuilder"></param>
        public static INClientSerializationBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpTransport<TClient>(
            this INClientTransportBuilder<TClient> clientAdvancedTransportBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedTransportBuilder, nameof(clientAdvancedTransportBuilder));

            return clientAdvancedTransportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientAdvancedTransportBuilder"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static INClientSerializationBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpTransport<TClient>(
            this INClientTransportBuilder<TClient> clientAdvancedTransportBuilder,
            IAuthenticator authenticator)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedTransportBuilder, nameof(clientAdvancedTransportBuilder));

            return clientAdvancedTransportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(authenticator),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientAdvancedTransportBuilder"></param>
        public static INClientFactorySerializationBuilder<IRestRequest, IRestResponse> UsingRestSharpTransport(
            this INClientFactoryTransportBuilder clientAdvancedTransportBuilder)
        {
            Ensure.IsNotNull(clientAdvancedTransportBuilder, nameof(clientAdvancedTransportBuilder));

            return clientAdvancedTransportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientAdvancedTransportBuilder"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static INClientFactorySerializationBuilder<IRestRequest, IRestResponse> UsingRestSharpTransport(
            this INClientFactoryTransportBuilder clientAdvancedTransportBuilder,
            IAuthenticator authenticator)
        {
            Ensure.IsNotNull(clientAdvancedTransportBuilder, nameof(clientAdvancedTransportBuilder));

            return clientAdvancedTransportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(authenticator),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }
    }
}
