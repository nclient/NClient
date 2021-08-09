using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Handling;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Core.Interceptors.HttpClients;
using NClient.Core.Interceptors.HttpResponsePopulation;
using NClient.Core.Interceptors.Invocation;
using NClient.Core.Interceptors.MethodBuilders;
using NClient.Core.Interceptors.MethodBuilders.Providers;
using NClient.Core.Interceptors.RequestBuilders;
using NClient.Core.Mappers;

namespace NClient.Core.Interceptors
{
    internal interface IClientInterceptorFactory
    {
        IAsyncInterceptor Create<TInterface>(
            Uri host,
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IReadOnlyCollection<IClientHandler> clientHandlers,
            IMethodResiliencePolicyProvider methodResiliencePolicyProvider,
            ILogger<TInterface>? logger = null);

        IAsyncInterceptor Create<TInterface, TController>(
            Uri host,
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IReadOnlyCollection<IClientHandler> clientHandlers,
            IMethodResiliencePolicyProvider methodResiliencePolicyProvider,
            ILogger<TInterface>? logger = null);
    }

    internal class ClientInterceptorFactory : IClientInterceptorFactory
    {
        private readonly IMethodBuilder _clientMethodBuilder;
        private readonly IFullMethodInvocationProvider _fullMethodInvocationProvider;
        private readonly IGuidProvider _guidProvider;
        private readonly IRequestBuilder _requestBuilder;
        private readonly IClientRequestExceptionFactory _clientRequestExceptionFactory;

        public ClientInterceptorFactory(
            IProxyGenerator proxyGenerator,
            IAttributeMapper attributeMapper)
        {
            _clientRequestExceptionFactory = new ClientRequestExceptionFactory();
            _fullMethodInvocationProvider = new FullMethodInvocationProvider(proxyGenerator);
            _guidProvider = new GuidProvider();

            var clientArgumentExceptionFactory = new ClientArgumentExceptionFactory();
            var clientValidationExceptionFactory = new ClientValidationExceptionFactory();
            var clientObjectMemberManagerExceptionFactory = new ClientObjectMemberManagerExceptionFactory();

            _clientMethodBuilder = new MethodBuilder(
                new MethodAttributeProvider(attributeMapper, clientValidationExceptionFactory),
                new UseVersionAttributeProvider(attributeMapper, clientValidationExceptionFactory),
                new PathAttributeProvider(attributeMapper, clientValidationExceptionFactory),
                new HeaderAttributeProvider(clientValidationExceptionFactory),
                new MethodParamBuilder(new ParamAttributeProvider(attributeMapper, clientValidationExceptionFactory)));

            var objectMemberManager = new ObjectMemberManager(clientObjectMemberManagerExceptionFactory);
            _requestBuilder = new RequestBuilder(
                new RouteTemplateProvider(clientValidationExceptionFactory),
                new RouteProvider(objectMemberManager, clientArgumentExceptionFactory, clientValidationExceptionFactory),
                new HttpMethodProvider(clientValidationExceptionFactory),
                new ObjectToKeyValueConverter(objectMemberManager, clientValidationExceptionFactory),
                clientValidationExceptionFactory);
        }

        public IAsyncInterceptor Create<TInterface>(
            Uri host,
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IReadOnlyCollection<IClientHandler> clientHandlers,
            IMethodResiliencePolicyProvider methodResiliencePolicyProvider,
            ILogger<TInterface>? logger = null)
        {
            return new ClientInterceptor<TInterface>(
                host,
                CreateResilienceHttpClientProvider(
                    httpClientProvider,
                    serializerProvider,
                    methodResiliencePolicyProvider,
                    logger),
                _fullMethodInvocationProvider,
                _clientRequestExceptionFactory,
                _clientMethodBuilder,
                _requestBuilder,
                new ClientHandlerDecorator<TInterface>(clientHandlers, logger),
                _guidProvider,
                controllerType: null,
                logger);
        }

        public IAsyncInterceptor Create<TInterface, TController>(
            Uri host,
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IReadOnlyCollection<IClientHandler> clientHandlers,
            IMethodResiliencePolicyProvider methodResiliencePolicyProvider,
            ILogger<TInterface>? logger = null)
        {
            return new ClientInterceptor<TInterface>(
                host,
                CreateResilienceHttpClientProvider(
                    httpClientProvider,
                    serializerProvider,
                    methodResiliencePolicyProvider,
                    logger),
                _fullMethodInvocationProvider,
                _clientRequestExceptionFactory,
                _clientMethodBuilder,
                _requestBuilder,
                new ClientHandlerDecorator<TInterface>(clientHandlers, logger),
                _guidProvider,
                controllerType: typeof(TController),
                logger);
        }

        private static IResilienceHttpClientProvider CreateResilienceHttpClientProvider<TInterface>(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IMethodResiliencePolicyProvider methodResiliencePolicyProvider,
            ILogger<TInterface>? logger)
        {
            return new ResilienceHttpClientProvider(
                httpClientProvider,
                serializerProvider,
                new HttpResponsePopulater(serializerProvider.Create()),
                methodResiliencePolicyProvider,
                logger);
        }
    }
}