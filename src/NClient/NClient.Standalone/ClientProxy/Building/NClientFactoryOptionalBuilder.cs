using System;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.Building;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Results;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Core.Proxy;
using NClient.Standalone.ClientProxy.Building.Configuration.Resilience;
using NClient.Standalone.ClientProxy.Building.Context;
using NClient.Standalone.ClientProxy.ClientGeneration;
using NClient.Standalone.ClientProxy.Interceptors;

namespace NClient.Standalone.ClientProxy.Building
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

        public INClientFactoryOptionalBuilder<TRequest, TResponse> EnsuringCustomSuccess(
            params IEnsuringSettings<TRequest, TResponse>[] ensuringSettings)
        {
            Ensure.IsNotNull(ensuringSettings, nameof(ensuringSettings));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithEnsuringSetting(ensuringSettings));
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> EnsuringCustomSuccess(
            Predicate<IResponseContext<TRequest, TResponse>> successCondition, Action<IResponseContext<TRequest, TResponse>> onFailure)
        {
            Ensure.IsNotNull(successCondition, nameof(successCondition));
            Ensure.IsNotNull(onFailure, nameof(onFailure));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithEnsuringSetting(successCondition, onFailure));
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> NotEnsuringSuccess()
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithoutEnsuringSetting());
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomSerialization(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithSerializer(serializerProvider));
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
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResults(params IResultBuilderProvider<IHttpResponse>[] resultBuilderProviders)
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
