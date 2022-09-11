using System.Linq;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Core.Helpers;
using NClient.Core.Mappers;
using NClient.Providers;
using NClient.Providers.Transport;
using NClient.Standalone.Client;
using NClient.Standalone.Client.Authorization;
using NClient.Standalone.Client.Handling;
using NClient.Standalone.Client.Logging;
using NClient.Standalone.Client.Mapping;
using NClient.Standalone.Client.Resilience;
using NClient.Standalone.Client.Validation;
using NClient.Standalone.ClientProxy.Building.Context;
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
        IAsyncInterceptor Create<TClient, TRequest, TResponse>(BuilderContext<TRequest, TResponse> builderContext);
    }

    internal class ClientInterceptorFactory : IClientInterceptorFactory
    {
        private readonly IProxyGenerator _proxyGenerator;
        private readonly ITimeoutSelector _timeoutSelector;
        private readonly IGuidProvider _guidProvider;
        private readonly IClientRequestExceptionFactory _clientRequestExceptionFactory;
        private readonly IMethodBuilder _methodBuilder;

        public ClientInterceptorFactory(IProxyGenerator proxyGenerator)
        {
            var clientValidationExceptionFactory = new ClientValidationExceptionFactory();
            var attributeMapper = new AttributeMapper();
            
            _clientRequestExceptionFactory = new ClientRequestExceptionFactory();
            _proxyGenerator = proxyGenerator;
            _timeoutSelector = new TimeoutSelector(clientValidationExceptionFactory);
            _guidProvider = new GuidProvider();
            
            _methodBuilder = new MethodBuilder(
                new OperationAttributeProvider(attributeMapper, clientValidationExceptionFactory),
                new UseVersionAttributeProvider(attributeMapper, clientValidationExceptionFactory),
                new PathAttributeProvider(attributeMapper, clientValidationExceptionFactory),
                new MetadataAttributeProvider(clientValidationExceptionFactory),
                new TimeoutAttributeProvider(attributeMapper, clientValidationExceptionFactory),
                new MethodParamBuilder(new ParamAttributeProvider(attributeMapper, clientValidationExceptionFactory)));
        }

        public IAsyncInterceptor Create<TClient, TRequest, TResponse>(BuilderContext<TRequest, TResponse> builderContext)
        {
            var logger = new CompositeLogger<TClient>(builderContext.LoggerFactory is not null
                ? builderContext.Loggers.Concat(new[] { builderContext.LoggerFactory.CreateLogger<TClient>() })
                : builderContext.Loggers);

            // TODO: Should be initialized in every request.
            var serializer = builderContext.SerializerProvider.Create(logger);
            var toolset = new Toolset(serializer, logger);

            var resiliencePolicyProvider = new StubResiliencePolicyProvider<TRequest, TResponse>();
            var authorizationProvider = new CompositeAuthorizationProvider(builderContext.AuthorizationProviders);
            var host = builderContext.Host;
            var clientHandlerProvider = new CompositeClientHandlerProvider<TRequest, TResponse>(builderContext.ClientHandlerProviders);
            var responseMapperProvider = new CompositeResponseMapperProvider<IRequest, IResponse>(builderContext.ResponseMapperProviders);
            var transportResponseMapperProvider = new CompositeResponseMapperProvider<TRequest, TResponse>(builderContext.TransportResponseMapperProviders);
            var compositeResponseValidatorProvider = new CompositeResponseValidatorProvider<TRequest, TResponse>(builderContext.ResponseValidatorProviders);
            var methodResiliencePolicyProviderAdapter = new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(
                new StubResiliencePolicyProvider<TRequest, TResponse>(),
                builderContext.MethodsWithResiliencePolicy.Reverse());
            
            return new ClientInterceptor<TClient, TRequest, TResponse>(
                _timeoutSelector,
                _guidProvider,
                _methodBuilder,
                new ExplicitMethodInvocationProvider<TRequest, TResponse>(_proxyGenerator),
                new ClientMethodInvocationProvider<TRequest, TResponse>(),
                authorizationProvider,
                host,
                builderContext.RequestBuilderProvider,
                new TransportNClientFactory<TRequest, TResponse>(
                    builderContext.TransportProvider,
                    builderContext.TransportRequestBuilderProvider,
                    builderContext.ResponseBuilderProvider,
                    clientHandlerProvider,
                    resiliencePolicyProvider,
                    responseMapperProvider,
                    transportResponseMapperProvider,
                    compositeResponseValidatorProvider,
                    toolset),
                methodResiliencePolicyProviderAdapter,
                _clientRequestExceptionFactory,
                builderContext.Timeout,
                toolset);
        }
    }
}
