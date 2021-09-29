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
        IAsyncInterceptor Create<TInterface, TRequest, TResponse>(
            Uri host,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory,
            ISerializerProvider serializerProvider,
            IReadOnlyCollection<IClientHandler<TRequest, TResponse>> clientHandlers,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            ILogger<TInterface>? logger = null);
    }

    internal class ClientInterceptorFactory : IClientInterceptorFactory
    {
        private readonly IProxyGenerator _proxyGenerator;
        private readonly IMethodBuilder _clientMethodBuilder;
        private readonly IGuidProvider _guidProvider;
        private readonly IRequestBuilder _requestBuilder;
        private readonly IClientRequestExceptionFactory _clientRequestExceptionFactory;

        public ClientInterceptorFactory(
            IProxyGenerator proxyGenerator,
            IAttributeMapper attributeMapper)
        {
            _proxyGenerator = proxyGenerator;
            _clientRequestExceptionFactory = new ClientRequestExceptionFactory();
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

        public IAsyncInterceptor Create<TInterface, TRequest, TResponse>(
            Uri host,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory,
            ISerializerProvider serializerProvider,
            IReadOnlyCollection<IClientHandler<TRequest, TResponse>> clientHandlers,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            ILogger<TInterface>? logger = null)
        {
            var serializer = serializerProvider.Create();
            var httpMessageBuilder = httpMessageBuilderProvider.Create(serializer);
            
            return new ClientInterceptor<TInterface, TRequest, TResponse>(
                host,
                CreateResilienceHttpClientProvider(
                    serializerProvider,
                    clientHandlers,
                    httpClientProvider,
                    httpMessageBuilder,
                    methodResiliencePolicyProvider,
                    logger),
                new FullMethodInvocationProvider<TRequest, TResponse>(_proxyGenerator),
                httpMessageBuilder,
                new HttpResponsePopulater(serializerProvider.Create()),
                _clientRequestExceptionFactory,
                _clientMethodBuilder,
                _requestBuilder,
                _guidProvider,
                logger);
        }
        
        private static IResilienceHttpClientProvider<TRequest, TResponse> CreateResilienceHttpClientProvider<TInterface, TRequest, TResponse>(
            ISerializerProvider serializerProvider,
            IReadOnlyCollection<IClientHandler<TRequest, TResponse>> clientHandlers,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilder<TRequest, TResponse> httpMessageBuilder,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            ILogger<TInterface>? logger)
        {
            return new ResilienceHttpClientProvider<TRequest, TResponse>(
                serializerProvider,
                new ClientHandlerDecorator<TInterface, TRequest, TResponse>(clientHandlers, logger),
                httpClientProvider,
                httpMessageBuilder,
                methodResiliencePolicyProvider,
                logger);
        }
    }
}
