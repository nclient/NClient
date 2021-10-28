using System;
using System.Net.Http;
using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Transport.Http.System.Stubs;

namespace NClient.Providers.Transport.Http.System
{
    /// <summary>
    /// The System.Net.Http based provider for a component that can create <see cref="ITransport{TRequest,TResponse}"/> instances.
    /// </summary>
    public class SystemHttpTransportProvider : ITransportProvider<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly Func<IHttpClientFactory> _httpClientFactory;
        private readonly string? _httpClientName;

        /// <summary>
        /// Creates the System.Net.Http based HTTP client provider.
        /// </summary>
        public SystemHttpTransportProvider()
        {
            _httpClientFactory = () => new StubHttpClientFactory(new HttpClientHandler());
        }

        /// <summary>
        /// Creates the System.Net.Http based HTTP client provider.
        /// </summary>
        /// <param name="httpMessageHandler">The HTTP message handler.</param>
        public SystemHttpTransportProvider(HttpMessageHandler httpMessageHandler)
        {
            Ensure.IsNotNull(httpMessageHandler, nameof(httpMessageHandler));

            _httpClientFactory = () => new StubHttpClientFactory(httpMessageHandler);
        }

        /// <summary>
        /// Creates the System.Net.Http based HTTP client provider.
        /// </summary>
        /// <param name="httpClient">The system <see cref="HttpClient"/>.</param>
        public SystemHttpTransportProvider(HttpClient httpClient)
        {
            Ensure.IsNotNull(httpClient, nameof(httpClient));

            _httpClientFactory = () => new StubHttpClientFactory(httpClient);
        }

        /// <summary>
        /// Creates the System.Net.Http based HTTP client provider.
        /// </summary>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="HttpClient"/> to create.</param>
        public SystemHttpTransportProvider(IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            _httpClientFactory = () => httpClientFactory;
            _httpClientName = httpClientName;
        }

        public ITransport<HttpRequestMessage, HttpResponseMessage> Create(ISerializer serializer)
        {
            Ensure.IsNotNull(serializer, nameof(serializer));

            return new SystemHttpTransport(_httpClientFactory.Invoke(), _httpClientName);
        }
    }
}
