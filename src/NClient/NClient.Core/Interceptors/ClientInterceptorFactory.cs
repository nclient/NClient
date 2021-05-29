using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Core.HttpClients;
using NClient.Core.Interceptors.ClientInvocations;
using NClient.Core.Mappers;
using NClient.Core.MethodBuilders;
using NClient.Core.MethodBuilders.Providers;
using NClient.Core.RequestBuilders;
using NClient.Core.Resilience;

namespace NClient.Core.Interceptors
{
    internal interface IClientInterceptorFactory
    {
        IAsyncInterceptor Create<TInterface>(
            Uri host,
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILogger<TInterface>? logger = null);

        IAsyncInterceptor Create<TInterface, TController>(
            Uri host,
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILogger<TInterface>? logger = null);
    }

    internal class ClientInterceptorFactory : IClientInterceptorFactory
    {
        private readonly IMethodBuilder _clientMethodBuilder;
        private readonly IClientInvocationProvider _clientInvocationProvider;
        private readonly IGuidProvider _guidProvider;
        private readonly IRequestBuilder _requestBuilder;

        public ClientInterceptorFactory(
            IProxyGenerator proxyGenerator,
            IAttributeMapper attributeMapper)
        {
            _clientInvocationProvider = new ClientInvocationProvider(proxyGenerator);
            _guidProvider = new GuidProvider();

            _clientMethodBuilder = new MethodBuilder(
                new MethodAttributeProvider(attributeMapper),
                new UseVersionAttributeProvider(attributeMapper),
                new PathAttributeProvider(attributeMapper),
                new HeaderAttributeProvider(),
                new MethodParamBuilder(new ParamAttributeProvider(attributeMapper)));

            var objectMemberManager = new ObjectMemberManager();
            _requestBuilder = new RequestBuilder(
                new RouteTemplateProvider(),
                new RouteProvider(objectMemberManager),
                new HttpMethodProvider(),
                new ObjectToKeyValueConverter(objectMemberManager));
        }

        public IAsyncInterceptor Create<TInterface>(
            Uri host,
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILogger<TInterface>? logger = null)
        {
            return new ClientInterceptor<TInterface>(
                host,
                CreateResilienceHttpClientProvider(
                    httpClientProvider,
                    serializerProvider,
                    resiliencePolicyProvider,
                    logger),
                _clientInvocationProvider,
                _clientMethodBuilder,
                _requestBuilder,
                _guidProvider,
                controllerType: null,
                logger);
        }

        public IAsyncInterceptor Create<TInterface, TController>(
            Uri host,
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILogger<TInterface>? logger = null)
        {
            return new ClientInterceptor<TInterface>(
                host,
                CreateResilienceHttpClientProvider(
                    httpClientProvider,
                    serializerProvider,
                    resiliencePolicyProvider,
                    logger),
                _clientInvocationProvider,
                _clientMethodBuilder,
                _requestBuilder,
                _guidProvider,
                controllerType: typeof(TController),
                logger);
        }

        private static IResilienceHttpClientProvider CreateResilienceHttpClientProvider<TInterface>(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILogger<TInterface>? logger = null)
        {
            return new ResilienceHttpClientProvider(
                httpClientProvider,
                serializerProvider,
                resiliencePolicyProvider ?? new StubResiliencePolicyProvider(),
                logger);
        }
    }
}