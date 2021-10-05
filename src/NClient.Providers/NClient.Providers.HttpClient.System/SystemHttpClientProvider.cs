﻿using System;
using System.Net.Http;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Providers.HttpClient.System.Stubs;

namespace NClient.Providers.HttpClient.System
{
    /// <summary>
    /// The System.Net.Http based provider for a component that can create <see cref="IHttpClient"/> instances.
    /// </summary>
    public class SystemHttpClientProvider : IHttpClientProvider<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly Func<IHttpClientFactory> _httpClientFactory;
        private readonly string? _httpClientName;

        /// <summary>
        /// Creates the System.Net.Http based HTTP client provider.
        /// </summary>
        public SystemHttpClientProvider()
        {
            _httpClientFactory = () => new StubHttpClientFactory(new HttpClientHandler());
        }

        /// <summary>
        /// Creates the System.Net.Http based HTTP client provider.
        /// </summary>
        /// <param name="httpMessageHandler">The HTTP message handler.</param>
        public SystemHttpClientProvider(HttpMessageHandler httpMessageHandler)
        {
            Ensure.IsNotNull(httpMessageHandler, nameof(httpMessageHandler));

            _httpClientFactory = () => new StubHttpClientFactory(httpMessageHandler);
        }

        /// <summary>
        /// Creates the System.Net.Http based HTTP client provider.
        /// </summary>
        /// <param name="httpClient">The system <see cref="System.Net.Http.HttpClient"/>.</param>
        public SystemHttpClientProvider(global::System.Net.Http.HttpClient httpClient)
        {
            Ensure.IsNotNull(httpClient, nameof(httpClient));

            _httpClientFactory = () => new StubHttpClientFactory(httpClient);
        }

        /// <summary>
        /// Creates the System.Net.Http based HTTP client provider.
        /// </summary>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="System.Net.Http.HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="System.Net.Http.HttpClient"/> to create.</param>
        public SystemHttpClientProvider(IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            _httpClientFactory = () => httpClientFactory;
            _httpClientName = httpClientName;
        }

        public IHttpClient<HttpRequestMessage, HttpResponseMessage> Create(ISerializer serializer)
        {
            Ensure.IsNotNull(serializer, nameof(serializer));

            return new SystemHttpClient(_httpClientFactory.Invoke(), _httpClientName);
        }
    }
}
