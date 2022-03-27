using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Core.Helpers;
using NClient.Core.Mappers;
using NClient.Providers;
using NClient.Providers.Api;
using NClient.Providers.Caching;
using NClient.Providers.Handling;
using NClient.Providers.Mapping;
using NClient.Providers.Resilience;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using NClient.Providers.Validation;
using NClient.Standalone.Client;
using NClient.Standalone.Client.Handling;
using NClient.Standalone.Client.Validation;
using NClient.Standalone.ClientProxy.Generation.Helpers;
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
            Uri host,
            ISerializerProvider serializerProvider,
            IRequestBuilderProvider requestBuilderProvider,
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportRequestBuilderProvider<TRequest, TResponse> transportRequestBuilderProvider,
            IResponseBuilderProvider<TRequest, TResponse> responseBuilderProvider,
            IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> clientHandlerProviders,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            IEnumerable<IResponseMapperProvider<IRequest, IResponse>> responseMapperProviders,
            IEnumerable<IResponseMapperProvider<TRequest, TResponse>> transportResponseMapperProviders,
            IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> responseValidatorProviders,
            IResponseCacheProvider? responseCacheProvider,
            IResponseCacheProvider? transportResponseCacheProvider,
            TimeSpan? timeout = null,
            ILogger<TClient>? logger = null);
    }

    internal class ClientInterceptorFactory : IClientInterceptorFactory
    {
        private readonly IProxyGenerator _proxyGenerator;
        private readonly ITimeoutSelector _timeoutSelector;
        private readonly IGuidProvider _guidProvider;
        private readonly IClientRequestExceptionFactory _clientRequestExceptionFactory;
        private readonly IAttributeMapper _attributeMapper;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public ClientInterceptorFactory(IProxyGenerator proxyGenerator)
        {
            _clientValidationExceptionFactory = new ClientValidationExceptionFactory();
            _clientRequestExceptionFactory = new ClientRequestExceptionFactory();
            _proxyGenerator = proxyGenerator;
            _timeoutSelector = new TimeoutSelector(_clientValidationExceptionFactory);
            _guidProvider = new GuidProvider();
            _attributeMapper = new AttributeMapper();
        }

        public IAsyncInterceptor Create<TClient, TRequest, TResponse>(
            Uri host,
            ISerializerProvider serializerProvider,
            IRequestBuilderProvider requestBuilderProvider,
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportRequestBuilderProvider<TRequest, TResponse> transportRequestBuilderProvider,
            IResponseBuilderProvider<TRequest, TResponse> responseBuilderProvider,
            IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> clientHandlerProviders,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            IEnumerable<IResponseMapperProvider<IRequest, IResponse>> responseMapperProviders,
            IEnumerable<IResponseMapperProvider<TRequest, TResponse>> transportResponseMapperProviders,
            IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> responseValidatorProviders,
            IResponseCacheProvider? responseCacheProvider,
            IResponseCacheProvider? transportResponseCacheProvider,
            TimeSpan? timeout,
            ILogger<TClient>? logger = null)
        {
            var methodBuilder = new MethodBuilder(
                new OperationAttributeProvider(_attributeMapper, _clientValidationExceptionFactory),
                new UseVersionAttributeProvider(_attributeMapper, _clientValidationExceptionFactory),
                new PathAttributeProvider(_attributeMapper, _clientValidationExceptionFactory),
                new MetadataAttributeProvider(_clientValidationExceptionFactory),
                new TimeoutAttributeProvider(_attributeMapper, _clientValidationExceptionFactory),
                new MethodParamBuilder(new ParamAttributeProvider(_attributeMapper, _clientValidationExceptionFactory)));
            
            var serializer = serializerProvider.Create(logger);
            var toolset = new Toolset(serializer, logger);
            
            return new ClientInterceptor<TClient, TRequest, TResponse>(
                host,
                _timeoutSelector,
                _guidProvider,
                methodBuilder,
                new ExplicitMethodInvocationProvider<TRequest, TResponse>(_proxyGenerator),
                new ClientMethodInvocationProvider<TRequest, TResponse>(),
                requestBuilderProvider.Create(toolset),
                new TransportNClientFactory<TRequest, TResponse>(
                    transportProvider,
                    transportRequestBuilderProvider,
                    responseBuilderProvider,
                    new ClientHandlerProviderDecorator<TRequest, TResponse>(clientHandlerProviders),
                    new StubResiliencePolicyProvider<TRequest, TResponse>(),
                    responseMapperProviders
                        .OrderByDescending(x => x is IOrderedResponseMapperProvider<TRequest, TResponse>)
                        .ThenBy(x => (x as IOrderedResponseMapperProvider<TRequest, TResponse>)?.Order)
                        .ToArray(),
                    transportResponseMapperProviders
                        .OrderByDescending(x => x is IOrderedResponseMapperProvider<TRequest, TResponse>)
                        .ThenBy(x => (x as IOrderedResponseMapperProvider<TRequest, TResponse>)?.Order)
                        .ToArray(),
                    new ResponseValidatorProviderDecorator<TRequest, TResponse>(responseValidatorProviders),
                    transportResponseCacheProvider,
                    toolset),
                methodResiliencePolicyProvider,
                _clientRequestExceptionFactory,
                responseCacheProvider?.Create(toolset),
                timeout,
                toolset);
        }
    }
}
