using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.Common.Helpers;
using NClient.Core.Proxy;
using NClient.Providers.Handling;
using NClient.Providers.Mapping;
using NClient.Providers.Serialization;
using NClient.Providers.Validation;
using NClient.Resilience;
using NClient.Standalone.Client.Logging;
using NClient.Standalone.ClientProxy.Building.Configuration.Handling;
using NClient.Standalone.ClientProxy.Building.Configuration.Mapping;
using NClient.Standalone.ClientProxy.Building.Configuration.Resilience;
using NClient.Standalone.ClientProxy.Building.Configuration.Validation;
using NClient.Standalone.ClientProxy.Building.Context;
using NClient.Standalone.ClientProxy.Generation;
using NClient.Standalone.ClientProxy.Generation.Interceptors;
using NClient.Standalone.ClientProxy.Validation;
using NClient.Standalone.ClientProxy.Validation.Resilience;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientOptionalBuilder<TClient, TRequest, TResponse> : INClientOptionalBuilder<TClient, TRequest, TResponse>
        where TClient : class
    {
        private readonly BuilderContext<TRequest, TResponse> _context;
        private readonly SingletonProxyGeneratorProvider _proxyGeneratorProvider;
        private readonly IClientInterceptorFactory _clientInterceptorFactory;
        private readonly IClientProxyGenerator _clientProxyGenerator;

        public NClientOptionalBuilder(BuilderContext<TRequest, TResponse> context)
        {
            _context = context;
            _proxyGeneratorProvider = new SingletonProxyGeneratorProvider();
            _clientInterceptorFactory = new ClientInterceptorFactory(_proxyGeneratorProvider.Value);
            _clientProxyGenerator = new ClientProxyGenerator(_proxyGeneratorProvider.Value);
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomSerialization(ISerializerProvider provider)
        {
            Ensure.IsNotNull(provider, nameof(provider));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithSerializer(provider));
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseValidation(IEnumerable<IResponseValidator<TRequest, TResponse>> validators)
        {
            return WithAdvancedResponseValidation(x => x
                .ForTransport().Use(validators));
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithAdvancedResponseValidation(Action<INClientResponseValidationSelector<TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModifier = new BuilderContextModifier<TRequest, TResponse>();
            configure(new NClientResponseValidationSelector<TRequest, TResponse>(builderContextModifier));
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(builderContextModifier.Invoke(_context));
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResponseValidation()
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithoutResponseValidation());
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithHandling(IEnumerable<IClientHandler<TRequest, TResponse>> handlers)
        {
            return WithAdvancedHandling(x => x
                .ForTransport().Use(handlers));
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithAdvancedHandling(Action<INClientHandlingSelector<TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModifier = new BuilderContextModifier<TRequest, TResponse>();
            configure(new NClientHandlingSelector<TRequest, TResponse>(builderContextModifier));
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(builderContextModifier.Invoke(_context));
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutHandling()
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithoutHandlers());
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseMapping(IEnumerable<IResponseMapper<TRequest, TResponse>> builders)
        {
            return WithAdvancedResponseMapping(x => x
                .ForTransport().Use(builders));
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithAdvancedResponseMapping(Action<INClientResponseMappingSelector<TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModifier = new BuilderContextModifier<TRequest, TResponse>();
            configure(new NClientResponseMappingSelector<TRequest, TResponse>(builderContextModifier));
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(builderContextModifier.Invoke(_context));
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResponseMapping()
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithoutResultBuilders());
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithResilience(Action<INClientResilienceMethodSelector<TClient, TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModifier = new BuilderContextModifier<TRequest, TResponse>();
            configure(new NClientResilienceMethodSelector<TClient, TRequest, TResponse>(builderContextModifier));
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(builderContextModifier.Invoke(_context));
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResilience()
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithoutResiliencePolicy());
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory)
        {
            Ensure.IsNotNull(loggerFactory, nameof(loggerFactory));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithLogging(loggerFactory));
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILogger logger, params ILogger[] extraLoggers)
        {
            Ensure.IsNotNull(logger, nameof(logger));
            Ensure.AreNotNullItems(extraLoggers, nameof(extraLoggers));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithLogging(extraLoggers.Concat(new[] { logger })));
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(IEnumerable<ILogger> loggers)
        {
            var loggerCollection = loggers as ICollection<ILogger> ?? loggers.ToArray();
            Ensure.AreNotNullItems(loggerCollection, nameof(loggerCollection));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithLogging(loggerCollection));
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutLogging()
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithoutLogging());
        }

        public TClient Build()
        {
            _context.EnsureComplete();
            new ClientValidator(_proxyGeneratorProvider.Value)
                .EnsureAsync<TClient>(_clientInterceptorFactory)
                .GetAwaiter()
                .GetResult();
            
            var interceptor = _clientInterceptorFactory.Create(
                _context.Host,
                _context.SerializerProvider,
                _context.RequestBuilderProvider,
                _context.TransportProvider,
                _context.TransportRequestBuilderProvider,
                _context.ResponseBuilderProvider,
                _context.ClientHandlerProviders,
                new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(
                    new StubResiliencePolicyProvider<TRequest, TResponse>(), 
                    _context.MethodsWithResiliencePolicy.Reverse()),
                _context.ResultBuilderProviders
                    .OrderByDescending(x => x is IOrderedResponseMapperProvider)
                    .ThenBy(x => (x as IOrderedResponseMapperProvider)?.Order)
                    .ToArray(),
                _context.TypedResultBuilderProviders
                    .OrderByDescending(x => x is IOrderedResponseMapperProvider)
                    .ThenBy(x => (x as IOrderedResponseMapperProvider)?.Order)
                    .ToArray(),
                _context.ResponseValidatorProviders,
                new LoggerDecorator<TClient>(_context.LoggerFactory is not null
                    ? _context.Loggers.Concat(new[] { _context.LoggerFactory.CreateLogger<TClient>() })
                    : _context.Loggers));

            return _clientProxyGenerator.CreateClient<TClient>(interceptor);
        }
    }
}
