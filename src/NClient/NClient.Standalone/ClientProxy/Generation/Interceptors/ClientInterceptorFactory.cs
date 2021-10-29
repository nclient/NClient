using System.Collections.Generic;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Core.Helpers;
using NClient.Core.Mappers;
using NClient.Providers.Api;
using NClient.Providers.Handling;
using NClient.Providers.Resilience;
using NClient.Providers.Results;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using NClient.Providers.Validation;
using NClient.Standalone.Client;
using NClient.Standalone.Client.Handling;
using NClient.Standalone.Client.Validation;
using NClient.Standalone.ClientProxy.Generation.Invocation;
using NClient.Standalone.ClientProxy.Generation.Invocation.MethodBuilders;
using NClient.Standalone.ClientProxy.Generation.Invocation.MethodBuilders.Providers;
using NClient.Standalone.ClientProxy.Validation.Resilience;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Generation.Interceptors
{
    internal interface IClientInterceptorFactory
    {
        IAsyncInterceptor Create<TClient, TRequest, TResponse>(
            string resourceRoot,
            ISerializerProvider serializerProvider,
            IRequestBuilderProvider requestBuilderProvider,
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
            string resourceRoot,
            ISerializerProvider serializerProvider,
            IRequestBuilderProvider requestBuilderProvider,
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
                resourceRoot,
                _guidProvider,
                _methodBuilder,
                new FullMethodInvocationProvider<TRequest, TResponse>(_proxyGenerator),
                requestBuilderProvider.Create(serializerProvider.Create()),
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
