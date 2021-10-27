using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.Common.Helpers;
using NClient.Core.Proxy;
using NClient.Providers.Handling;
using NClient.Providers.Resilience;
using NClient.Providers.Results;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using NClient.Providers.Validation;
using NClient.Standalone.Client.Handling;
using NClient.Standalone.Client.Validation;
using NClient.Standalone.ClientProxy.Building.Configuration.Resilience;
using NClient.Standalone.ClientProxy.Building.Context;
using NClient.Standalone.ClientProxy.ClientGeneration;
using NClient.Standalone.ClientProxy.Interceptors;

namespace NClient.Standalone.ClientProxy.Building.Factory
{
    internal class NClientFactoryOptionalBuilder<TRequest, TResponse> : INClientFactoryOptionalBuilder<TRequest, TResponse>
    {
        private readonly string _factoryName;
        private readonly BuilderContext<TRequest, TResponse> _context;
        private readonly IClientInterceptorFactory _clientInterceptorFactory;
        private readonly IClientGenerator _clientGenerator;

        public NClientFactoryOptionalBuilder(string factoryName, BuilderContext<TRequest, TResponse> context)
        {
            _factoryName = factoryName;
            _context = context;
            
            var proxyGeneratorProvider = new SingletonProxyGeneratorProvider();
            _clientInterceptorFactory = new ClientInterceptorFactory(proxyGeneratorProvider.Value);
            _clientGenerator = new ClientGenerator(proxyGeneratorProvider.Value);
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
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomHandling(params IClientHandlerSettings<TRequest, TResponse>[] clientHandlerSettings)
        {
            return WithCustomHandling(clientHandlerSettings
                .Select(x => new ClientHandler<TRequest, TResponse>(x))
                .Cast<IClientHandler<TRequest, TResponse>>()
                .ToArray());
        }
        
        /// <summary>
        /// Sets collection of <see cref="IClientHandler{TRequest,TResponse}"/> used to handle HTTP requests and responses />.
        /// </summary>
        /// <param name="handlers">The collection of handlers.</param>
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomHandling(params IClientHandler<TRequest, TResponse>[] handlers)
        {
            return WithCustomHandling(handlers
                .Select(x => new ClientHandlerProvider<TRequest, TResponse>(x))
                .Cast<IClientHandlerProvider<TRequest, TResponse>>()
                .ToArray());
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomHandling(params IClientHandlerProvider<TRequest, TResponse>[] providers)
        {
            Ensure.IsNotNull(providers, nameof(providers));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithHandlers(providers));
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
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResults(params IResultBuilderProvider<IResponse>[] resultBuilderProviders)
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithResultBuilders(resultBuilderProviders));
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResults(params IResultBuilderProvider<TResponse>[] resultBuilderProviders)
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
            return new CustomNClientFactory<TRequest, TResponse>(_factoryName, _context);
        }
    }
}
