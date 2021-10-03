using System.Net.Http;
using NClient.Abstractions.Builders;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public static class HttpClientCustomizerExtensions
    {
        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="clientHttpClientBuilder"></param>
        public static INClientSerializerBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingSystemHttpClient<TClient>(
            this INClientHttpClientBuilder<TClient> clientHttpClientBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientHttpClientBuilder, nameof(clientHttpClientBuilder));

            return clientHttpClientBuilder.UsingCustomHttpClient(
                new SystemHttpClientProvider(), 
                new SystemHttpMessageBuilderProvider(),
                new SystemHttpClientExceptionFactory());
        }
        
        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="factoryHttpClientBuilder"></param>
        public static INClientFactorySerializerBuilder<HttpRequestMessage, HttpResponseMessage> UsingSystemHttpClient(
            this INClientFactoryHttpClientBuilder factoryHttpClientBuilder)
        {
            Ensure.IsNotNull(factoryHttpClientBuilder, nameof(factoryHttpClientBuilder));

            return factoryHttpClientBuilder.UsingCustomHttpClient(
                new SystemHttpClientProvider(), 
                new SystemHttpMessageBuilderProvider(),
                new SystemHttpClientExceptionFactory());
        }
        
        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="clientHttpClientBuilder"></param>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="System.Net.Http.HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="System.Net.Http.HttpClient"/> to create.</param>
        public static INClientSerializerBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingSystemHttpClient<TClient>(
            this INClientHttpClientBuilder<TClient> clientHttpClientBuilder,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
            where TClient : class
        {
            Ensure.IsNotNull(clientHttpClientBuilder, nameof(clientHttpClientBuilder));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return clientHttpClientBuilder.UsingCustomHttpClient(
                new SystemHttpClientProvider(httpClientFactory, httpClientName), 
                new SystemHttpMessageBuilderProvider(),
                new SystemHttpClientExceptionFactory());
        }
        
        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="factoryHttpClientBuilder"></param>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="System.Net.Http.HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="System.Net.Http.HttpClient"/> to create.</param>
        public static INClientFactorySerializerBuilder<HttpRequestMessage, HttpResponseMessage> UsingSystemHttpClient(
            this INClientFactoryHttpClientBuilder factoryHttpClientBuilder,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            Ensure.IsNotNull(factoryHttpClientBuilder, nameof(factoryHttpClientBuilder));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return factoryHttpClientBuilder.UsingCustomHttpClient(
                new SystemHttpClientProvider(httpClientFactory, httpClientName), 
                new SystemHttpMessageBuilderProvider(),
                new SystemHttpClientExceptionFactory());
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="clientHttpClientBuilder"></param>
        /// <param name="httpMessageHandler">The HTTP message handler.</param>
        public static INClientSerializerBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingSystemHttpClient<TClient>(
            this INClientHttpClientBuilder<TClient> clientHttpClientBuilder,
            HttpMessageHandler httpMessageHandler)
            where TClient : class
        {
            Ensure.IsNotNull(clientHttpClientBuilder, nameof(clientHttpClientBuilder));
            Ensure.IsNotNull(httpMessageHandler, nameof(httpMessageHandler));

            return clientHttpClientBuilder.UsingCustomHttpClient(
                new SystemHttpClientProvider(httpMessageHandler), 
                new SystemHttpMessageBuilderProvider(),
                new SystemHttpClientExceptionFactory());
        }
        
        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="factoryHttpClientBuilder"></param>
        /// <param name="httpMessageHandler">The HTTP message handler.</param>
        public static INClientFactorySerializerBuilder<HttpRequestMessage, HttpResponseMessage> UsingSystemHttpClient(
            this INClientFactoryHttpClientBuilder factoryHttpClientBuilder,
            HttpMessageHandler httpMessageHandler)
        {
            Ensure.IsNotNull(factoryHttpClientBuilder, nameof(factoryHttpClientBuilder));
            Ensure.IsNotNull(httpMessageHandler, nameof(httpMessageHandler));

            return factoryHttpClientBuilder.UsingCustomHttpClient(
                new SystemHttpClientProvider(httpMessageHandler), 
                new SystemHttpMessageBuilderProvider(),
                new SystemHttpClientExceptionFactory());
        }
    }
}
