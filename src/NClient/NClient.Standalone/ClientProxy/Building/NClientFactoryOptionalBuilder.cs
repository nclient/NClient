using System;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.Builders;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Results;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Core.Proxy;
using NClient.Resilience;
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
            IEnsuringSettings<TRequest, TResponse> ensuringSettings)
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

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomHandling(params IClientHandler<TRequest, TResponse>[] handlers)
        {
            Ensure.IsNotNull(handlers, nameof(handlers));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithHandlers(handlers));
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutHandling()
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithoutHandlers());
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithFullResilience(IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            Ensure.IsNotNull(provider, nameof(provider));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithResiliencePolicy(new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(provider)));
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithIdempotentResilience(
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(idempotentMethodProvider, nameof(idempotentMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithResiliencePolicy(new IdempotentMethodResiliencePolicyProvider<TRequest, TResponse>(idempotentMethodProvider, otherMethodProvider)));
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithSafeResilience(
            IResiliencePolicyProvider<TRequest, TResponse> safeMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(safeMethodProvider, nameof(safeMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithResiliencePolicy(new SafeMethodResiliencePolicyProvider<TRequest, TResponse>(safeMethodProvider, otherMethodProvider)));
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

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithLogging(ILogger logger)
        {
            Ensure.IsNotNull(logger, nameof(logger));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithLogging(logger));
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
