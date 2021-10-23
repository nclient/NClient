using NClient.Common.Helpers;
using NClient.Providers.Transport;
using NClient.Providers.Transport.Http.RestSharp;
using RestSharp;
using RestSharp.Authenticators;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class HttpClientBuilderExtensions
    {
        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientHttpClientBuilder"></param>
        public static INClientSerializerBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpHttpClient<TClient>(
            this INClientHttpClientBuilder<TClient> clientHttpClientBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientHttpClientBuilder, nameof(clientHttpClientBuilder));

            return clientHttpClientBuilder.UsingCustomHttpClient(
                new RestSharpTransportProvider(),
                new RestSharpHttpMessageBuilderProvider());
        }
        
        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="factoryHttpClientBuilder"></param>
        public static INClientFactorySerializerBuilder<IRestRequest, IRestResponse> UsingRestSharpHttpClient(
            this INClientFactoryHttpClientBuilder factoryHttpClientBuilder)
        {
            Ensure.IsNotNull(factoryHttpClientBuilder, nameof(factoryHttpClientBuilder));

            return factoryHttpClientBuilder.UsingCustomHttpClient(
                new RestSharpTransportProvider(),
                new RestSharpHttpMessageBuilderProvider());
        }

        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientHttpClientBuilder"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static INClientSerializerBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpHttpClient<TClient>(
            this INClientHttpClientBuilder<TClient> clientHttpClientBuilder,
            IAuthenticator authenticator)
            where TClient : class
        {
            Ensure.IsNotNull(clientHttpClientBuilder, nameof(clientHttpClientBuilder));

            return clientHttpClientBuilder.UsingCustomHttpClient(
                new RestSharpTransportProvider(authenticator),
                new RestSharpHttpMessageBuilderProvider());
        }
        
        /// <summary>
        /// Sets RestSharp based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="factoryHttpClientBuilder"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static INClientFactorySerializerBuilder<IRestRequest, IRestResponse> UsingRestSharpHttpClient(
            this INClientFactoryHttpClientBuilder factoryHttpClientBuilder,
            IAuthenticator authenticator)
        {
            Ensure.IsNotNull(factoryHttpClientBuilder, nameof(factoryHttpClientBuilder));

            return factoryHttpClientBuilder.UsingCustomHttpClient(
                new RestSharpTransportProvider(authenticator),
                new RestSharpHttpMessageBuilderProvider());
        }
    }
}
