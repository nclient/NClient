using System.Net.Http;
using NClient.Abstractions.Building;
using NClient.Abstractions.Providers.Transport;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class HttpClientBuilderExtensions
    {
        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider{TRequest,TResponse}"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="clientHttpClientBuilder"></param>
        public static INClientSerializerBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingHttpClient<TClient>(
            this INClientHttpClientBuilder<TClient> clientHttpClientBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientHttpClientBuilder, nameof(clientHttpClientBuilder));

            return clientHttpClientBuilder.UsingSystemHttpClient();
        }
        
        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider{TRequest,TResponse}"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="factoryHttpClientBuilder"></param>
        public static INClientFactorySerializerBuilder<HttpRequestMessage, HttpResponseMessage> UsingHttpClient(
            this INClientFactoryHttpClientBuilder factoryHttpClientBuilder)
        {
            Ensure.IsNotNull(factoryHttpClientBuilder, nameof(factoryHttpClientBuilder));

            return factoryHttpClientBuilder.UsingSystemHttpClient();
        }
    }
}
