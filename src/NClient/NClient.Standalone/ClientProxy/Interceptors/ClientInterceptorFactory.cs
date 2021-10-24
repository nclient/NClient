using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Mappers;
using NClient.Providers.Handling;
using NClient.Providers.Resilience;
using NClient.Providers.Results;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using NClient.Providers.Validation;
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
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportMessageBuilderProvider<TRequest, TResponse> transportMessageBuilderProvider,
            IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> clientHandlerProviders,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            IEnumerable<IResultBuilderProvider<IResponse>> resultBuilderProviders,
            IEnumerable<IResultBuilderProvider<TResponse>> typedResultBuilderProviders,
            IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> responseValidatorProviders,
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
                new RequestTypeProvider(clientValidationExceptionFactory),
                new ObjectToKeyValueConverter(objectMemberManager, clientValidationExceptionFactory),
                clientValidationExceptionFactory);
        }

        public IAsyncInterceptor Create<TClient, TRequest, TResponse>(
            Uri host,
            ISerializerProvider serializerProvider,
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportMessageBuilderProvider<TRequest, TResponse> transportMessageBuilderProvider,
            IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> clientHandlerProviders,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            IEnumerable<IResultBuilderProvider<IResponse>> resultBuilderProviders,
            IEnumerable<IResultBuilderProvider<TResponse>> typedResultBuilderProviders,
            IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> responseValidatorProviders,
            ILogger<TClient>? logger = null)
        {
            return new ClientInterceptor<TClient, TRequest, TResponse>(
                host,
                _guidProvider,
                _methodBuilder,
                new FullMethodInvocationProvider<TRequest, TResponse>(_proxyGenerator),
                _requestBuilder,
                new TransportNClientFactory<TRequest, TResponse>(
                    serializerProvider,
                    transportProvider,
                    transportMessageBuilderProvider,
                    new ClientHandlerProviderDecorator<TRequest, TResponse>(clientHandlerProviders),
                    new StubResiliencePolicyProvider<TRequest, TResponse>(),
                    resultBuilderProviders,
                    typedResultBuilderProviders,
                    new ResponseValidatorProviderDecorator<TRequest, TResponse>(responseValidatorProviders),
                    logger),
                methodResiliencePolicyProvider,
                _clientRequestExceptionFactory,
                logger);
        }
    }
}
