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
        public static INClientAdvancedSerializerBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpTransport<TClient>(
            this INClientAdvancedTransportBuilder<TClient> clientAdvancedTransportBuilder)
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
        /// <param name="clientTransportBuilder"></param>
        public static INClientSerializerBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpTransport<TClient>(
            this INClientTransportBuilder<TClient> clientTransportBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientTransportBuilder, nameof(clientTransportBuilder));

            return UsingRestSharpTransport(clientTransportBuilder.AsAdvanced()).AsBasic();
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
        /// <param name="clientAdvancedTransportBuilder"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static INClientAdvancedSerializerBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpTransport<TClient>(
            this INClientAdvancedTransportBuilder<TClient> clientAdvancedTransportBuilder,
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
        /// <param name="clientTransportBuilder"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static INClientSerializerBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpTransport<TClient>(
            this INClientTransportBuilder<TClient> clientTransportBuilder,
            IAuthenticator authenticator)
            where TClient : class
        {
            Ensure.IsNotNull(clientTransportBuilder, nameof(clientTransportBuilder));

            return UsingRestSharpTransport(clientTransportBuilder.AsAdvanced(), authenticator).AsBasic();
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
