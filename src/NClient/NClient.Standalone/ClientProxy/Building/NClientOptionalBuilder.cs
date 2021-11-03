using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.Common.Helpers;
using NClient.Core.Proxy;
using NClient.Providers.Resilience;
using NClient.Providers.Results;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using NClient.Providers.Validation;
using NClient.Resilience;
using NClient.Standalone.Client.Logging;
using NClient.Standalone.Client.Validation;
using NClient.Standalone.ClientProxy.Building.Configuration.Handling;
using NClient.Standalone.ClientProxy.Building.Configuration.Resilience;
using NClient.Standalone.ClientProxy.Building.Context;
using NClient.Standalone.ClientProxy.Generation;
using NClient.Standalone.ClientProxy.Generation.Interceptors;
using NClient.Standalone.ClientProxy.Validation;
using NClient.Standalone.ClientProxy.Validation.Resilience;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientOptionalBuilder<TClient, TRequest, TResponse> : INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse>
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
        
        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomResponseValidation(params IResponseValidatorSettings<TRequest, TResponse>[] responseValidatorSettings)
        {
            return WithCustomResponseValidation(responseValidatorSettings
                .Select(x => new ResponseValidator<TRequest, TResponse>(x))
                .Cast<IResponseValidator<TRequest, TResponse>>()
                .ToArray());
        }

        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomResponseValidation(params IResponseValidator<TRequest, TResponse>[] responseValidators)
        {
            return WithCustomResponseValidation(responseValidators
                .Select(x => new ResponseValidatorProvider<TRequest, TResponse>(x))
                .Cast<IResponseValidatorProvider<TRequest, TResponse>>()
                .ToArray());
        }

        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomResponseValidation(params IResponseValidatorProvider<TRequest, TResponse>[] responseValidatorProvider)
        {
            Ensure.IsNotNull(responseValidatorProvider, nameof(responseValidatorProvider));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithResponseValidation(responseValidatorProvider));
        }

        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithoutResponseValidation()
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithoutResponseValidation());
        }

        INClientOptionalBuilder<TClient, TRequest, TResponse> INClientOptionalBuilder<TClient, TRequest, TResponse>.WithoutResponseValidation()
        {
            return WithoutResponseValidation();
        }

        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomSerialization(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithSerializer(serializerProvider));
        }
        
        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomHandling(Action<INClientAdvancedHandlingSetter<TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModifier = new BuilderContextModifier<TRequest, TResponse>();
            configure(new NClientAdvancedHandlingSetter<TRequest, TResponse>(builderContextModifier));
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(builderContextModifier.Invoke(_context));
        }
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> INClientOptionalBuilder<TClient, TRequest, TResponse>.WithHandling(Action<INClientHandlingSetter<TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModifier = new BuilderContextModifier<TRequest, TResponse>();
            configure(new NClientHandlingSetter<TRequest, TResponse>(builderContextModifier));
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(builderContextModifier.Invoke(_context));
        }

        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithoutHandling()
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithoutHandlers());
        }

        INClientOptionalBuilder<TClient, TRequest, TResponse> INClientOptionalBuilder<TClient, TRequest, TResponse>.WithoutHandling()
        {
            return WithoutHandling();
        }

        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomResilience(IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider)
        {
            Ensure.IsNotNull(methodResiliencePolicyProvider, nameof(methodResiliencePolicyProvider));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithResiliencePolicy(methodResiliencePolicyProvider));
        }
        
        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithResilience(Action<INClientResilienceMethodSelector<TClient, TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModifier = new BuilderContextModifier<TRequest, TResponse>();
            configure(new NClientResilienceMethodSelector<TClient, TRequest, TResponse>(builderContextModifier));
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(builderContextModifier.Invoke(_context));
        }
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> INClientOptionalBuilder<TClient, TRequest, TResponse>.WithResilience(Action<INClientResilienceMethodSelector<TClient, TRequest, TResponse>> configure)
        {
            return WithResilience(configure);
        }
        
        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithoutResilience()
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithoutResiliencePolicy());
        }
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> INClientOptionalBuilder<TClient, TRequest, TResponse>.WithoutResilience()
        {
            return WithoutResilience();
        }
        
        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomResults(params IResultBuilderProvider<IRequest, IResponse>[] resultBuilderProviders)
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithResultBuilders(resultBuilderProviders));
        }     
        
        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomResults(params IResultBuilderProvider<TRequest, TResponse>[] resultBuilderProviders)
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithResultBuilders(resultBuilderProviders));
        }
        
        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithoutCustomResults()
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithoutResultBuilders());
        }
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> INClientOptionalBuilder<TClient, TRequest, TResponse>.WithoutCustomResults()
        {
            return WithoutCustomResults();
        }

        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory)
        {
            Ensure.IsNotNull(loggerFactory, nameof(loggerFactory));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithLogging(loggerFactory));
        }
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> INClientOptionalBuilder<TClient, TRequest, TResponse>.WithLogging(ILoggerFactory loggerFactory)
        {
            return this.AsAdvanced().WithLogging(loggerFactory);
        }

        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithLogging(params ILogger[] loggers)
        {
            Ensure.IsNotNull(loggers, nameof(loggers));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithLogging(loggers));
        }
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> INClientOptionalBuilder<TClient, TRequest, TResponse>.WithLogging(params ILogger[] loggers)
        {
            return this.AsAdvanced().WithLogging(loggers);
        }
        
        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithoutLogging()
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithoutLogging());
        }
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> INClientOptionalBuilder<TClient, TRequest, TResponse>.WithoutLogging()
        {
            return this.AsAdvanced().WithoutLogging();
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
                    .OrderByDescending(x => x is IOrderedResultBuilderProvider)
                    .ThenBy(x => (x as IOrderedResultBuilderProvider)?.Order)
                    .ToArray(),
                _context.TypedResultBuilderProviders
                    .OrderByDescending(x => x is IOrderedResultBuilderProvider)
                    .ThenBy(x => (x as IOrderedResultBuilderProvider)?.Order)
                    .ToArray(),
                _context.ResponseValidatorProviders,
                new LoggerDecorator<TClient>(_context.LoggerFactory is not null
                    ? _context.Loggers.Concat(new[] { _context.LoggerFactory.CreateLogger<TClient>() })
                    : _context.Loggers));

            return _clientProxyGenerator.CreateClient<TClient>(interceptor);
        }

        TClient INClientOptionalBuilder<TClient, TRequest, TResponse>.Build()
        {
            return Build();
        }
    }
}
