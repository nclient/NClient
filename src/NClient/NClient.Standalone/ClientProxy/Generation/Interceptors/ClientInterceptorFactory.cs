﻿using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Core.Helpers;
using NClient.Core.Mappers;
using NClient.Providers;
using NClient.Providers.Api;
using NClient.Providers.Handling;
using NClient.Providers.Mapping;
using NClient.Providers.Resilience;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using NClient.Providers.Validation;
using NClient.Standalone.Client;
using NClient.Standalone.Client.Handling;
using NClient.Standalone.Client.Validation;
using NClient.Standalone.ClientProxy.Generation.Invocation;
using NClient.Standalone.ClientProxy.Generation.MethodBuilders;
using NClient.Standalone.ClientProxy.Generation.MethodBuilders.Providers;
using NClient.Standalone.ClientProxy.Validation.Resilience;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Generation.Interceptors
{
    internal interface IClientInterceptorFactory
    {
        IAsyncInterceptor Create<TClient, TRequest, TResponse>(
            string resource,
            ISerializerProvider serializerProvider,
            IRequestBuilderProvider requestBuilderProvider,
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportRequestBuilderProvider<TRequest, TResponse> transportRequestBuilderProvider,
            IResponseBuilderProvider<TRequest, TResponse> responseBuilderProvider,
            IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> clientHandlerProviders,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            IEnumerable<IResponseMapperProvider<IRequest, IResponse>> resultBuilderProviders,
            IEnumerable<IResponseMapperProvider<TRequest, TResponse>> typedResultBuilderProviders,
            IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> responseValidatorProviders,
            ILogger<TClient>? logger = null);
    }

    internal class ClientInterceptorFactory : IClientInterceptorFactory
    {
        private readonly IMethodBuilder _methodBuilder;
        private readonly IProxyGenerator _proxyGenerator;
        private readonly IGuidProvider _guidProvider;
        private readonly IClientRequestExceptionFactory _clientRequestExceptionFactory;

        public ClientInterceptorFactory(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
            _guidProvider = new GuidProvider();
            _clientRequestExceptionFactory = new ClientRequestExceptionFactory();

            var attributeMapper = new AttributeMapper();
            var clientValidationExceptionFactory = new ClientValidationExceptionFactory();

            _methodBuilder = new MethodBuilder(
                new OperationAttributeProvider(attributeMapper, clientValidationExceptionFactory),
                new UseVersionAttributeProvider(attributeMapper, clientValidationExceptionFactory),
                new PathAttributeProvider(attributeMapper, clientValidationExceptionFactory),
                new MetadataAttributeProvider(clientValidationExceptionFactory),
                new MethodParamBuilder(new ParamAttributeProvider(attributeMapper, clientValidationExceptionFactory)));
        }

        public IAsyncInterceptor Create<TClient, TRequest, TResponse>(
            string resource,
            ISerializerProvider serializerProvider,
            IRequestBuilderProvider requestBuilderProvider,
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportRequestBuilderProvider<TRequest, TResponse> transportRequestBuilderProvider,
            IResponseBuilderProvider<TRequest, TResponse> responseBuilderProvider,
            IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> clientHandlerProviders,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            IEnumerable<IResponseMapperProvider<IRequest, IResponse>> resultBuilderProviders,
            IEnumerable<IResponseMapperProvider<TRequest, TResponse>> typedResultBuilderProviders,
            IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> responseValidatorProviders,
            ILogger<TClient>? logger = null)
        {
            var serializer = serializerProvider.Create(logger);
            var toolset = new Toolset(serializer, logger);
            
            return new ClientInterceptor<TClient, TRequest, TResponse>(
                resource,
                _guidProvider,
                _methodBuilder,
                new ExplicitInvocationProvider<TRequest, TResponse>(_proxyGenerator),
                requestBuilderProvider.Create(toolset),
                new TransportNClientFactory<TRequest, TResponse>(
                    transportProvider,
                    transportRequestBuilderProvider,
                    responseBuilderProvider,
                    new ClientHandlerProviderDecorator<TRequest, TResponse>(clientHandlerProviders),
                    new StubResiliencePolicyProvider<TRequest, TResponse>(),
                    resultBuilderProviders
                        .OrderByDescending(x => x is IOrderedResponseMapperProvider)
                        .ThenBy(x => (x as IOrderedResponseMapperProvider)?.Order)
                        .ToArray(),
                    typedResultBuilderProviders
                        .OrderByDescending(x => x is IOrderedResponseMapperProvider)
                        .ThenBy(x => (x as IOrderedResponseMapperProvider)?.Order)
                        .ToArray(),
                    new ResponseValidatorProviderDecorator<TRequest, TResponse>(responseValidatorProviders),
                    toolset),
                methodResiliencePolicyProvider,
                _clientRequestExceptionFactory,
                toolset);
        }
    }
}
