using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Core.HttpClients;
using NClient.Core.Interceptors;
using NClient.Core.Interceptors.ClientInvocations;
using NClient.Core.Mappers;
using NClient.Core.MethodBuilders;
using NClient.Core.MethodBuilders.Providers;
using NClient.Core.RequestBuilders;
using NClient.Core.Resilience;

namespace NClient.InterfaceBasedClients
{
    internal class InterfaceBasedClientBuilder<TInterface> : IInterfaceBasedClientBuilder<TInterface> where TInterface : class
    {
        private readonly Uri _host;
        private readonly IProxyGenerator _proxyGenerator;
        private IHttpClientProvider _httpClientProvider;
        private ISerializerProvider _serializerProvider;
        private IResiliencePolicyProvider? _resiliencePolicyProvider;
        private ILogger<TInterface>? _logger;

        public InterfaceBasedClientBuilder(
            Uri host, IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider, IProxyGenerator proxyGenerator)
        {
            _host = host;
            _httpClientProvider = httpClientProvider;
            _serializerProvider = serializerProvider;
            _proxyGenerator = proxyGenerator;
        }

        public IInterfaceBasedClientBuilder<TInterface> WithCustomHttpClient(IHttpClientProvider httpClientProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));

            _httpClientProvider = httpClientProvider;
            return this;
        }

        public IInterfaceBasedClientBuilder<TInterface> WithCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            _serializerProvider = serializerProvider;
            return this;
        }

        public IInterfaceBasedClientBuilder<TInterface> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            Ensure.IsNotNull(resiliencePolicyProvider, nameof(resiliencePolicyProvider));

            _resiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public IInterfaceBasedClientBuilder<TInterface> WithLogging(ILogger<TInterface> logger)
        {
            Ensure.IsNotNull(logger, nameof(logger));

            _logger = logger;
            return this;
        }

        public TInterface Build()
        {
            var clientInvocationProvider = new ClientInvocationProvider(_proxyGenerator);
            var objectMemberManager = new ObjectMemberManager();
            var attributeMapper = new AttributeMapper();
            var guidProvider = new GuidProvider();

            var methodAttributeProvider = new MethodAttributeProvider(attributeMapper);
            var pathAttributeProvider = new PathAttributeProvider(attributeMapper);
            var useVersionAttributeProvider = new UseVersionAttributeProvider(attributeMapper);
            var headerAttributeProvider = new HeaderAttributeProvider();
            var paramAttributeProvider = new ParamAttributeProvider(attributeMapper);

            var clientMethodParamBuilder = new MethodParamBuilder(paramAttributeProvider);
            var clientMethodBuilder = new MethodBuilder(
                methodAttributeProvider,
                useVersionAttributeProvider,
                pathAttributeProvider,
                headerAttributeProvider,
                clientMethodParamBuilder);

            var requestBuilder = new RequestBuilder(
                _host,
                new RouteTemplateProvider(),
                new RouteProvider(objectMemberManager),
                new HttpMethodProvider(),
                new ObjectToKeyValueConverter(objectMemberManager));

            var resilienceHttpClientProvider = new ResilienceHttpClientProvider(
                _httpClientProvider,
                _serializerProvider,
                _resiliencePolicyProvider ?? new StubResiliencePolicyProvider(),
                _logger);

            var interceptor = new ClientInterceptor<TInterface>(
                resilienceHttpClientProvider,
                clientInvocationProvider,
                clientMethodBuilder,
                requestBuilder,
                guidProvider,
                controllerType: null,
                _logger);

            return (TInterface)_proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToProxy: typeof(TInterface),
                additionalInterfacesToProxy: new[] { typeof(IResilienceNClient<TInterface>), typeof(IHttpNClient<TInterface>) },
                interceptors: interceptor.ToInterceptor());
        }
    }
}
