using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
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
    public interface IInterfaceBasedClientBuilder<T> where T : class
    {
        IInterfaceBasedClientBuilder<T> SetCustomSerializer(ISerializerProvider serializerProvider);
        IInterfaceBasedClientBuilder<T> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        IInterfaceBasedClientBuilder<T> WithLogging(ILogger<T> logger);
        T Build();
    }

    internal class InterfaceBasedClientBuilder<T> : IInterfaceBasedClientBuilder<T> where T : class
    {
        private readonly Uri _host;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IProxyGenerator _proxyGenerator;
        private ISerializerProvider _serializerProvider;
        private IResiliencePolicyProvider? _resiliencePolicyProvider;
        private ILogger<T>? _logger;

        public InterfaceBasedClientBuilder(
            Uri host, IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider, IProxyGenerator proxyGenerator)
        {
            _host = host;
            _httpClientProvider = httpClientProvider;
            _serializerProvider = serializerProvider;
            _proxyGenerator = proxyGenerator;
        }

        public IInterfaceBasedClientBuilder<T> SetCustomSerializer(ISerializerProvider serializerProvider)
        {
            _serializerProvider = serializerProvider;
            return this;
        }

        public IInterfaceBasedClientBuilder<T> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            _resiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public IInterfaceBasedClientBuilder<T> WithLogging(ILogger<T> logger)
        {
            _logger = logger;
            return this;
        }

        public T Build()
        {
            var clientInvocationProvider = new ClientInvocationProvider(_proxyGenerator);
            var objectMemberManager = new ObjectMemberManager();
            var attributeMapper = new AttributeMapper();
            var guidProvider = new GuidProvider();

            var methodAttributeProvider = new MethodAttributeProvider(attributeMapper);
            var pathAttributeProvider = new PathAttributeProvider(attributeMapper);
            var headerAttributeProvider = new HeaderAttributeProvider();
            var paramAttributeProvider = new ParamAttributeProvider(attributeMapper);

            var clientMethodParamBuilder = new MethodParamBuilder(paramAttributeProvider);
            var clientMethodBuilder = new MethodBuilder(
                methodAttributeProvider,
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

            var interceptor = new ClientInterceptor<T>(
                resilienceHttpClientProvider,
                clientInvocationProvider,
                clientMethodBuilder,
                requestBuilder,
                guidProvider,
                controllerType: null,
                _logger);

            return (T)_proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToProxy: typeof(T),
                additionalInterfacesToProxy: new[] { typeof(IResilienceNClient<T>), typeof(IHttpNClient<T>) },
                interceptors: interceptor.ToInterceptor());
        }
    }
}
