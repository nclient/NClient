using System.Net.Http;
using NClient.Common.Helpers;
using NClient.Providers.Transport;
using NClient.Providers.Transport.Http.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SystemHttpTransportBuilderExtensions
    {
        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientAdvancedTransportBuilder"></param>
        public static INClientSerializationBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport<TClient>(
            this INClientTransportBuilder<TClient> clientAdvancedTransportBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedTransportBuilder, nameof(clientAdvancedTransportBuilder));

            return clientAdvancedTransportBuilder
                .UsingCustomTransport(
                    new SystemHttpTransportProvider(), 
                    new SystemHttpTransportRequestBuilderProvider(),
                    new SystemHttpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientAdvancedTransportBuilder"></param>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="HttpClient"/> to create.</param>
        public static INClientSerializationBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport<TClient>(
            this INClientTransportBuilder<TClient> clientAdvancedTransportBuilder,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedTransportBuilder, nameof(clientAdvancedTransportBuilder));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return clientAdvancedTransportBuilder.UsingCustomTransport(
                new SystemHttpTransportProvider(httpClientFactory, httpClientName), 
                new SystemHttpTransportRequestBuilderProvider(),
                new SystemHttpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientAdvancedTransportBuilder"></param>
        public static INClientFactorySerializationBuilder<HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport(
            this INClientFactoryTransportBuilder clientAdvancedTransportBuilder)
        {
            Ensure.IsNotNull(clientAdvancedTransportBuilder, nameof(clientAdvancedTransportBuilder));

            return clientAdvancedTransportBuilder
                .UsingCustomTransport(
                    new SystemHttpTransportProvider(), 
                    new SystemHttpTransportRequestBuilderProvider(),
                    new SystemHttpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientAdvancedTransportBuilder"></param>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="HttpClient"/> to create.</param>
        public static INClientFactorySerializationBuilder<HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport(
            this INClientFactoryTransportBuilder clientAdvancedTransportBuilder,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            Ensure.IsNotNull(clientAdvancedTransportBuilder, nameof(clientAdvancedTransportBuilder));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return clientAdvancedTransportBuilder.UsingCustomTransport(
                new SystemHttpTransportProvider(httpClientFactory, httpClientName), 
                new SystemHttpTransportRequestBuilderProvider(),
                new SystemHttpResponseBuilderProvider());
        }
    }
}
