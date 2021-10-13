﻿using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Results;
using NClient.Abstractions.Serialization;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Mappers;
using NClient.Standalone.Client;
using NClient.Standalone.Client.Handling;
using NClient.Standalone.Client.Resilience;
using NClient.Standalone.Client.Validation;
using NClient.Standalone.ClientProxy.Interceptors.Invocation;
using NClient.Standalone.ClientProxy.Interceptors.MethodBuilders;
using NClient.Standalone.ClientProxy.Interceptors.MethodBuilders.Providers;
using NClient.Standalone.ClientProxy.Interceptors.RequestBuilders;
using NClient.Standalone.Exceptions.Factories;
using NClient.Standalone.Helpers.ObjectToKeyValueConverters;

namespace NClient.Standalone.ClientProxy.Interceptors
{
    internal interface IClientInterceptorFactory
    {
        IAsyncInterceptor Create<TClient, TRequest, TResponse>(
            Uri host,
            ISerializerProvider serializerProvider,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IReadOnlyCollection<IClientHandler<TRequest, TResponse>> clientHandlers,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            IEnumerable<IResultBuilderProvider<IHttpResponse>> resultBuilderProviders,
            IEnumerable<IResultBuilderProvider<TResponse>> typedResultBuilderProviders,
            IResponseValidator<TRequest, TResponse> responseValidator,
            ILogger<TClient>? logger = null);
    }

    internal class ClientInterceptorFactory : IClientInterceptorFactory
    {
        private readonly IProxyGenerator _proxyGenerator;
        private readonly IMethodBuilder _methodBuilder;
        private readonly IGuidProvider _guidProvider;
        private readonly IRequestBuilder _requestBuilder;
        private readonly IClientRequestExceptionFactory _clientRequestExceptionFactory;

        public ClientInterceptorFactory(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
            _clientRequestExceptionFactory = new ClientRequestExceptionFactory();
            _guidProvider = new GuidProvider();

            var clientArgumentExceptionFactory = new ClientArgumentExceptionFactory();
            var clientValidationExceptionFactory = new ClientValidationExceptionFactory();
            var clientObjectMemberManagerExceptionFactory = new ClientObjectMemberManagerExceptionFactory();
            var attributeMapper = new AttributeMapper();

            _methodBuilder = new MethodBuilder(
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

        public IAsyncInterceptor Create<TClient, TRequest, TResponse>(
            Uri host,
            ISerializerProvider serializerProvider,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IReadOnlyCollection<IClientHandler<TRequest, TResponse>> clientHandlers,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            IEnumerable<IResultBuilderProvider<IHttpResponse>> resultBuilderProviders,
            IEnumerable<IResultBuilderProvider<TResponse>> typedResultBuilderProviders,
            IResponseValidator<TRequest, TResponse> responseValidator,
            ILogger<TClient>? logger = null)
        {
            return new ClientInterceptor<TClient, TRequest, TResponse>(
                host,
                _guidProvider,
                _methodBuilder,
                new FullMethodInvocationProvider<TRequest, TResponse>(_proxyGenerator),
                _requestBuilder,
                new HttpNClientFactory<TRequest, TResponse>(
                    serializerProvider,
                    httpClientProvider,
                    httpMessageBuilderProvider,
                    new ClientHandlerDecorator<TClient, TRequest, TResponse>(clientHandlers, logger),
                    new StubResiliencePolicyProvider<TRequest, TResponse>(),
                    resultBuilderProviders,
                    typedResultBuilderProviders,
                    responseValidator,
                    logger),
                methodResiliencePolicyProvider,
                _clientRequestExceptionFactory,
                logger);
        }
    }
}
