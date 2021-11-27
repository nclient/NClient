using System.Net.Http;
using NClient.Common.Helpers;
using NClient.Providers.Transport;
using NClient.Providers.Transport.SystemNetHttp;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SystemHttpTransportBuilderExtensions
    {
        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportBuilder"></param>
        public static INClientSerializationBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport<TClient>(
            this INClientTransportBuilder<TClient> transportBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder
                .UsingCustomTransport(
                    new SystemHttpTransportProvider(), 
                    new SystemHttpTransportRequestBuilderProvider(),
                    new SystemHttpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportBuilder"></param>
        /// <param name="httpClient">The custom HTTP client.</param>
        public static INClientSerializationBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport<TClient>(
            this INClientTransportBuilder<TClient> transportBuilder,
            HttpClient httpClient)
            where TClient : class
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder
                .UsingCustomTransport(
                    new SystemHttpTransportProvider(httpClient), 
                    new SystemHttpTransportRequestBuilderProvider(),
                    new SystemHttpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportBuilder"></param>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="HttpClient"/> to create.</param>
        public static INClientSerializationBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport<TClient>(
            this INClientTransportBuilder<TClient> transportBuilder,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
            where TClient : class
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return transportBuilder.UsingCustomTransport(
                new SystemHttpTransportProvider(httpClientFactory, httpClientName), 
                new SystemHttpTransportRequestBuilderProvider(),
                new SystemHttpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportBuilder"></param>
        public static INClientFactorySerializationBuilder<HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport(
            this INClientFactoryTransportBuilder transportBuilder)
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder
                .UsingCustomTransport(
                    new SystemHttpTransportProvider(), 
                    new SystemHttpTransportRequestBuilderProvider(),
                    new SystemHttpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportBuilder"></param>
        /// <param name="httpClient">The custom HTTP client.</param>
        public static INClientFactorySerializationBuilder<HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport(
            this INClientFactoryTransportBuilder transportBuilder,
            HttpClient httpClient)
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder
                .UsingCustomTransport(
                    new SystemHttpTransportProvider(httpClient), 
                    new SystemHttpTransportRequestBuilderProvider(),
                    new SystemHttpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportBuilder"></param>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="HttpClient"/> to create.</param>
        public static INClientFactorySerializationBuilder<HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport(
            this INClientFactoryTransportBuilder transportBuilder,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return transportBuilder.UsingCustomTransport(
                new SystemHttpTransportProvider(httpClientFactory, httpClientName), 
                new SystemHttpTransportRequestBuilderProvider(),
                new SystemHttpResponseBuilderProvider());
        }
    }
}
