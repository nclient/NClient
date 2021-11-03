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
        /// <param name="clientTransportBuilder"></param>
        public static INClientSerializerBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport<TClient>(
            this INClientTransportBuilder<TClient> clientTransportBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientTransportBuilder, nameof(clientTransportBuilder));

            return clientTransportBuilder.AsAdvanced()
                .UsingCustomTransport(
                    new SystemHttpTransportProvider(), 
                    new SystemHttpTransportRequestBuilderProvider(),
                    new SystemHttpResponseBuilderProvider());
        }
        
        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="factoryTransportBuilder"></param>
        public static INClientFactorySerializerBuilder<HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport(
            this INClientFactoryTransportBuilder factoryTransportBuilder)
        {
            Ensure.IsNotNull(factoryTransportBuilder, nameof(factoryTransportBuilder));

            return factoryTransportBuilder.UsingCustomTransport(
                new SystemHttpTransportProvider(), 
                new SystemHttpTransportRequestBuilderProvider(),
                new SystemHttpResponseBuilderProvider());
        }
        
        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientTransportBuilder"></param>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="HttpClient"/> to create.</param>
        public static INClientSerializerBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport<TClient>(
            this INClientTransportBuilder<TClient> clientTransportBuilder,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
            where TClient : class
        {
            Ensure.IsNotNull(clientTransportBuilder, nameof(clientTransportBuilder));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return clientTransportBuilder.AsAdvanced()
                .UsingCustomTransport(
                    new SystemHttpTransportProvider(httpClientFactory, httpClientName), 
                    new SystemHttpTransportRequestBuilderProvider(),
                    new SystemHttpResponseBuilderProvider());
        }
        
        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="factoryTransportBuilder"></param>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="HttpClient"/> to create.</param>
        public static INClientFactorySerializerBuilder<HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport(
            this INClientFactoryTransportBuilder factoryTransportBuilder,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            Ensure.IsNotNull(factoryTransportBuilder, nameof(factoryTransportBuilder));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return factoryTransportBuilder.UsingCustomTransport(
                new SystemHttpTransportProvider(httpClientFactory, httpClientName), 
                new SystemHttpTransportRequestBuilderProvider(),
                new SystemHttpResponseBuilderProvider());
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientTransportBuilder"></param>
        /// <param name="httpMessageHandler">The HTTP message handler.</param>
        public static INClientSerializerBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport<TClient>(
            this INClientTransportBuilder<TClient> clientTransportBuilder,
            HttpMessageHandler httpMessageHandler)
            where TClient : class
        {
            Ensure.IsNotNull(clientTransportBuilder, nameof(clientTransportBuilder));
            Ensure.IsNotNull(httpMessageHandler, nameof(httpMessageHandler));

            return clientTransportBuilder.AsAdvanced()
                .UsingCustomTransport(
                    new SystemHttpTransportProvider(httpMessageHandler), 
                    new SystemHttpTransportRequestBuilderProvider(),
                    new SystemHttpResponseBuilderProvider());
        }
        
        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="factoryTransportBuilder"></param>
        /// <param name="httpMessageHandler">The HTTP message handler.</param>
        public static INClientFactorySerializerBuilder<HttpRequestMessage, HttpResponseMessage> UsingSystemHttpTransport(
            this INClientFactoryTransportBuilder factoryTransportBuilder,
            HttpMessageHandler httpMessageHandler)
        {
            Ensure.IsNotNull(factoryTransportBuilder, nameof(factoryTransportBuilder));
            Ensure.IsNotNull(httpMessageHandler, nameof(httpMessageHandler));

            return factoryTransportBuilder.UsingCustomTransport(
                new SystemHttpTransportProvider(httpMessageHandler), 
                new SystemHttpTransportRequestBuilderProvider(),
                new SystemHttpResponseBuilderProvider());
        }
    }
}
