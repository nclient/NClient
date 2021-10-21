using NClient.Abstractions.Building;
using NClient.Abstractions.Providers.HttpClient;
using NClient.Common.Helpers;
using NClient.Providers.HttpClient.RestSharp;
using RestSharp;
using RestSharp.Authenticators;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class HttpClientBuilderExtensions
    {
        /// <summary>
        /// Sets RestSharp based <see cref="IHttpClientProvider{TRequest,TResponse}"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="clientHttpClientBuilder"></param>
        public static INClientSerializerBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpHttpClient<TClient>(
            this INClientHttpClientBuilder<TClient> clientHttpClientBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientHttpClientBuilder, nameof(clientHttpClientBuilder));

            return clientHttpClientBuilder.UsingCustomHttpClient(
                new RestSharpHttpClientProvider(),
                new RestSharpHttpMessageBuilderProvider());
        }
        
        /// <summary>
        /// Sets RestSharp based <see cref="IHttpClientProvider{TRequest,TResponse}"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="factoryHttpClientBuilder"></param>
        public static INClientFactorySerializerBuilder<IRestRequest, IRestResponse> UsingRestSharpHttpClient(
            this INClientFactoryHttpClientBuilder factoryHttpClientBuilder)
        {
            Ensure.IsNotNull(factoryHttpClientBuilder, nameof(factoryHttpClientBuilder));

            return factoryHttpClientBuilder.UsingCustomHttpClient(
                new RestSharpHttpClientProvider(),
                new RestSharpHttpMessageBuilderProvider());
        }

        /// <summary>
        /// Sets RestSharp based <see cref="IHttpClientProvider{TRequest,TResponse}"/> used to create instance of <see cref="IHttpClient"/>.
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
                new RestSharpHttpClientProvider(authenticator),
                new RestSharpHttpMessageBuilderProvider());
        }
        
        /// <summary>
        /// Sets RestSharp based <see cref="IHttpClientProvider{TRequest,TResponse}"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="factoryHttpClientBuilder"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static INClientFactorySerializerBuilder<IRestRequest, IRestResponse> UsingRestSharpHttpClient(
            this INClientFactoryHttpClientBuilder factoryHttpClientBuilder,
            IAuthenticator authenticator)
        {
            Ensure.IsNotNull(factoryHttpClientBuilder, nameof(factoryHttpClientBuilder));

            return factoryHttpClientBuilder.UsingCustomHttpClient(
                new RestSharpHttpClientProvider(authenticator),
                new RestSharpHttpMessageBuilderProvider());
        }
    }
}
