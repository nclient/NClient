using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Mapping;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Mappers;
using NClient.Standalone.Exceptions.Factories;
using NClient.Standalone.Handling;
using NClient.Standalone.Helpers.ObjectToKeyValueConverters;
using NClient.Standalone.Interceptors.HttpClients;
using NClient.Standalone.Interceptors.Invocation;
using NClient.Standalone.Interceptors.MethodBuilders;
using NClient.Standalone.Interceptors.MethodBuilders.Providers;
using NClient.Standalone.Interceptors.RequestBuilders;
using NClient.Standalone.Interceptors.Validation;

namespace NClient.Standalone.Interceptors
{
    internal interface IClientInterceptorFactory
    {
        IAsyncInterceptor Create<TClient, TRequest, TResponse>(
            Uri host,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            ISerializerProvider serializerProvider,
            IResponseValidator<TRequest, TResponse> responseValidator,
            IReadOnlyCollection<IClientHandler<TRequest, TResponse>> clientHandlers,
            IReadOnlyCollection<IResponseMapper> responseMappers,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            ILogger<TClient>? logger = null);
    }

    internal class ClientInterceptorFactory : IClientInterceptorFactory
    {
        private readonly IProxyGenerator _proxyGenerator;
        private readonly IMethodBuilder _clientMethodBuilder;
        private readonly IGuidProvider _guidProvider;
        private readonly IRequestBuilder _requestBuilder;

        public ClientInterceptorFactory(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
            _guidProvider = new GuidProvider();

            var clientArgumentExceptionFactory = new ClientArgumentExceptionFactory();
            var clientValidationExceptionFactory = new ClientValidationExceptionFactory();
            var clientObjectMemberManagerExceptionFactory = new ClientObjectMemberManagerExceptionFactory();
            var attributeMapper = new AttributeMapper();

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

        public IAsyncInterceptor Create<TClient, TRequest, TResponse>(
            Uri host,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            ISerializerProvider serializerProvider,
            IResponseValidator<TRequest, TResponse> responseValidator,
            IReadOnlyCollection<IClientHandler<TRequest, TResponse>> clientHandlers,
            IReadOnlyCollection<IResponseMapper> responseMappers,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            ILogger<TClient>? logger = null)
        {
            var serializer = serializerProvider.Create();
            var httpMessageBuilder = httpMessageBuilderProvider.Create(serializer);
            
            return new ClientInterceptor<TClient, TRequest, TResponse>(
                host,
                httpMessageBuilder,
                CreateResilienceHttpClientProvider(
                    httpClientProvider,
                    httpMessageBuilder,
                    serializerProvider,
                    clientHandlers,
                    methodResiliencePolicyProvider,
                    logger),
                new FullMethodInvocationProvider<TRequest, TResponse>(_proxyGenerator),
                serializerProvider,
                responseMappers,
                responseValidator,
                new ClientRequestExceptionFactory<TResponse>(),
                _clientMethodBuilder,
                _requestBuilder,
                _guidProvider,
                logger);
        }
        
        private static IResilienceHttpClientProvider<TRequest, TResponse> CreateResilienceHttpClientProvider<TClient, TRequest, TResponse>(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilder<TRequest, TResponse> httpMessageBuilder,
            ISerializerProvider serializerProvider,
            IReadOnlyCollection<IClientHandler<TRequest, TResponse>> clientHandlers,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            ILogger<TClient>? logger)
        {
            return new ResilienceHttpClientProvider<TRequest, TResponse>(
                serializerProvider,
                new ClientHandlerDecorator<TClient, TRequest, TResponse>(clientHandlers, logger),
                httpClientProvider,
                httpMessageBuilder,
                methodResiliencePolicyProvider,
                logger);
        }
    }
}
