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
        /// <param name="transportBuilder"></param>
        public static INClientSerializationBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpTransport<TClient>(
            this INClientTransportBuilder<TClient> transportBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportBuilder"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static INClientSerializationBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpTransport<TClient>(
            this INClientTransportBuilder<TClient> transportBuilder,
            IAuthenticator authenticator)
            where TClient : class
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(authenticator),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportBuilder"></param>
        public static INClientFactorySerializationBuilder<IRestRequest, IRestResponse> UsingRestSharpTransport(
            this INClientFactoryTransportBuilder transportBuilder)
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportBuilder"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static INClientFactorySerializationBuilder<IRestRequest, IRestResponse> UsingRestSharpTransport(
            this INClientFactoryTransportBuilder transportBuilder,
            IAuthenticator authenticator)
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(authenticator),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }
    }
}
