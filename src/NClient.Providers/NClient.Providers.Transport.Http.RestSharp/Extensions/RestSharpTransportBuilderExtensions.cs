using NClient.Common.Helpers;
using NClient.Providers.Transport;
using NClient.Providers.Transport.Http.RestSharp;
using RestSharp;
using RestSharp.Authenticators;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class RestSharpTransportBuilderExtensions
    {
        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientTransportBuilder"></param>
        public static INClientSerializerBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpTransport<TClient>(
            this INClientTransportBuilder<TClient> clientTransportBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientTransportBuilder, nameof(clientTransportBuilder));

            return clientTransportBuilder.AsAdvanced()
                .UsingCustomTransport(
                    new RestSharpTransportProvider(),
                    new RestSharpTransportRequestBuilderProvider(),
                    new RestSharpResponseBuilderProvider());
        }
        
        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="factoryTransportBuilder"></param>
        public static INClientFactorySerializerBuilder<IRestRequest, IRestResponse> UsingRestSharpTransport(
            this INClientFactoryTransportBuilder factoryTransportBuilder)
        {
            Ensure.IsNotNull(factoryTransportBuilder, nameof(factoryTransportBuilder));

            return factoryTransportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientTransportBuilder"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static INClientSerializerBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpTransport<TClient>(
            this INClientTransportBuilder<TClient> clientTransportBuilder,
            IAuthenticator authenticator)
            where TClient : class
        {
            Ensure.IsNotNull(clientTransportBuilder, nameof(clientTransportBuilder));

            return clientTransportBuilder.AsAdvanced()
                .UsingCustomTransport(
                    new RestSharpTransportProvider(authenticator),
                    new RestSharpTransportRequestBuilderProvider(),
                    new RestSharpResponseBuilderProvider());
        }
        
        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="factoryTransportBuilder"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static INClientFactorySerializerBuilder<IRestRequest, IRestResponse> UsingRestSharpTransport(
            this INClientFactoryTransportBuilder factoryTransportBuilder,
            IAuthenticator authenticator)
        {
            Ensure.IsNotNull(factoryTransportBuilder, nameof(factoryTransportBuilder));

            return factoryTransportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(authenticator),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }
    }
}
