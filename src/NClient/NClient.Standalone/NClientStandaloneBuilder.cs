using System;
using Castle.DynamicProxy;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.Mappers;
using NClient.Core.Validation;
using NClient.Mappers;
using NClient.OptionalNClientBuilders;

namespace NClient
{
    public class NClientStandaloneBuilder : INClientBuilder
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();

        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ISerializerProvider _serializerProvider;
        private readonly IClientValidator _clientValidator;
        private readonly IClientInterceptorFactory _interfaceClientInterceptorFactory;
        private readonly IClientInterceptorFactory _controllerClientInterceptorFactory;

        public NClientStandaloneBuilder(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            _httpClientProvider = httpClientProvider;
            _serializerProvider = serializerProvider;
            _clientValidator = new ClientValidator(ProxyGenerator);
            _interfaceClientInterceptorFactory = new ClientInterceptorFactory(ProxyGenerator, new AttributeMapper());
            _controllerClientInterceptorFactory = new ClientInterceptorFactory(ProxyGenerator, new AspNetAttributeMapper());
        }

        public IOptionalNClientBuilder<TInterface> Use<TInterface>(string host)
            where TInterface : class
        {
            Ensure.IsNotNull(host, nameof(host));
            _clientValidator.Ensure<TInterface>(_interfaceClientInterceptorFactory);

            return new OptionalInterfaceNClientBuilder<TInterface>(
                host: new Uri(host),
                ProxyGenerator,
                _interfaceClientInterceptorFactory,
                _httpClientProvider,
                _serializerProvider);
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public IOptionalNClientBuilder<TInterface> Use<TInterface, TController>(string host)
            where TInterface : class
            where TController : TInterface
        {
            Ensure.IsNotNull(host, nameof(host));
            _clientValidator.Ensure<TInterface, TController>(_controllerClientInterceptorFactory);

            return new OptionalControllerNClientBuilder<TInterface, TController>(
                host: new Uri(host),
                ProxyGenerator,
                _controllerClientInterceptorFactory,
                _httpClientProvider,
                _serializerProvider);
        }
    }
}
