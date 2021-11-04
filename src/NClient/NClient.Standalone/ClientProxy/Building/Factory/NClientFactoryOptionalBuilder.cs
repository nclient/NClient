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
using NClient.Standalone.Client.Validation;
using NClient.Standalone.ClientProxy.Building.Configuration.Handling;
using NClient.Standalone.ClientProxy.Building.Configuration.Resilience;
using NClient.Standalone.ClientProxy.Building.Context;
using NClient.Standalone.ClientProxy.Generation;
using NClient.Standalone.ClientProxy.Generation.Interceptors;

namespace NClient.Standalone.ClientProxy.Building.Factory
{
    internal class NClientFactoryOptionalBuilder<TRequest, TResponse> : INClientFactoryOptionalBuilder<TRequest, TResponse>
    {
        private readonly string _factoryName;
        private readonly BuilderContext<TRequest, TResponse> _context;
        private readonly IClientInterceptorFactory _clientInterceptorFactory;
        private readonly IClientProxyGenerator _clientProxyGenerator;

        public NClientFactoryOptionalBuilder(string factoryName, BuilderContext<TRequest, TResponse> context)
        {
            _factoryName = factoryName;
            _context = context;
            
            var proxyGeneratorProvider = new SingletonProxyGeneratorProvider();
            _clientInterceptorFactory = new ClientInterceptorFactory(proxyGeneratorProvider.Value);
            _clientProxyGenerator = new ClientProxyGenerator(proxyGeneratorProvider.Value);
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResponseValidation(params IResponseValidatorSettings<TRequest, TResponse>[] responseValidatorSettings)
        {
            return WithCustomResponseValidation(responseValidatorSettings
                .Select(x => new ResponseValidator<TRequest, TResponse>(x))
                .Cast<IResponseValidator<TRequest, TResponse>>()
                .ToArray());
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResponseValidation(params IResponseValidator<TRequest, TResponse>[] responseValidators)
        {
            return WithCustomResponseValidation(responseValidators
                .Select(x => new ResponseValidatorProvider<TRequest, TResponse>(x))
                .Cast<IResponseValidatorProvider<TRequest, TResponse>>()
                .ToArray());
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResponseValidation(params IResponseValidatorProvider<TRequest, TResponse>[] responseValidatorProviders)
        {
            Ensure.IsNotNull(responseValidatorProviders, nameof(responseValidatorProviders));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithResponseValidation(responseValidatorProviders));
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutResponseValidation()
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithoutResponseValidation());
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomSerialization(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithSerializer(serializerProvider));
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithHandling(Action<INClientAdvancedHandlingSetter<TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModifier = new BuilderContextModifier<TRequest, TResponse>();
            configure(new NClientAdvancedHandlingSetter<TRequest, TResponse>(builderContextModifier));
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, builderContextModifier.Invoke(_context));
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutHandling()
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithoutHandlers());
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResilience(IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider)
        {
            Ensure.IsNotNull(methodResiliencePolicyProvider, nameof(methodResiliencePolicyProvider));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithResiliencePolicy(methodResiliencePolicyProvider));
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResilience(Action<INClientFactoryResilienceMethodSelector<TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModifier = new BuilderContextModifier<TRequest, TResponse>();
            configure(new NClientFactoryResilienceMethodSelector<TRequest, TResponse>(builderContextModifier));
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, builderContextModifier.Invoke(_context));
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutResilience()
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithoutResiliencePolicy());
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResults(params IResultBuilderProvider<IRequest, IResponse>[] resultBuilderProviders)
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithResultBuilders(resultBuilderProviders));
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResults(params IResultBuilderProvider<TRequest, TResponse>[] resultBuilderProviders)
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithResultBuilders(resultBuilderProviders));
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutCustomResults()
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithoutResultBuilders());
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory)
        {
            Ensure.IsNotNull(loggerFactory, nameof(loggerFactory));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithLogging(loggerFactory));
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithLogging(params ILogger[] loggers)
        {
            Ensure.IsNotNull(loggers, nameof(loggers));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithLogging(loggers));
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutLogging()
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithoutLogging());
        }

        public INClientFactory Build()
        {
            return new NClientAdvancedFactory<TRequest, TResponse>(_factoryName, _context);
        }
    }
}
