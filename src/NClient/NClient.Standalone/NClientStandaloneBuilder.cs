using System;
using Castle.DynamicProxy;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.ClientGeneration;
using NClient.Common.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.Mappers;
using NClient.Core.Validation;
using NClient.Mappers;
using NClient.OptionalNClientBuilders;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client with custom providers.
    /// </summary>
    public class NClientStandaloneBuilder : INClientBuilder
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();

        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ISerializerProvider _serializerProvider;
        private readonly IClientValidator _clientValidator;
        private readonly IClientInterceptorFactory _interfaceClientInterceptorFactory;
        private readonly IClientInterceptorFactory _controllerClientInterceptorFactory;
        private readonly IClientGenerator _clientGenerator;

        /// <summary>
        /// Creates the builder with custom providers.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/> instances.</param>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/> instances.</param>
        public NClientStandaloneBuilder(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            _httpClientProvider = httpClientProvider;
            _serializerProvider = serializerProvider;
            _clientValidator = new ClientValidator(ProxyGenerator);
            _clientGenerator = new ClientGenerator(ProxyGenerator);
            _interfaceClientInterceptorFactory = new ClientInterceptorFactory(ProxyGenerator, new AttributeMapper());
            _controllerClientInterceptorFactory = new ClientInterceptorFactory(ProxyGenerator, new AspNetAttributeMapper());
        }

        public IOptionalNClientBuilder<TInterface> Use<TInterface>(string host)
            where TInterface : class
        {
            Ensure.IsNotNull(host, nameof(host));
            _clientValidator
                .EnsureAsync<TInterface>(_interfaceClientInterceptorFactory)
                .GetAwaiter()
                .GetResult();

            return new OptionalInterfaceNClientBuilder<TInterface>(
                host: new Uri(host),
                _clientGenerator,
                _interfaceClientInterceptorFactory,
                _httpClientProvider,
                _serializerProvider);
        }
    }
}
