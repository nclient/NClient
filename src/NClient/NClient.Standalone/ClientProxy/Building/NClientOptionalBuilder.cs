using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Builders;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Handling;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Core.Proxy;
using NClient.Resilience;
using NClient.Standalone.Client.Ensuring;
using NClient.Standalone.Client.Resilience;
using NClient.Standalone.ClientProxy.Building.Configuration.Resilience;
using NClient.Standalone.ClientProxy.Building.Context;
using NClient.Standalone.ClientProxy.ClientGeneration;
using NClient.Standalone.ClientProxy.Interceptors;
using NClient.Standalone.ClientProxy.Interceptors.Validation;
using NClient.Standalone.ClientProxy.Validation;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientOptionalBuilder<TClient, TRequest, TResponse> : INClientOptionalBuilder<TClient, TRequest, TResponse>
        where TClient : class
    {
        private readonly BuilderContext<TRequest, TResponse> _context;
        private readonly SingletonProxyGeneratorProvider _proxyGeneratorProvider;
        private readonly IClientInterceptorFactory _clientInterceptorFactory;
        private readonly IClientGenerator _clientGenerator;

        public NClientOptionalBuilder(BuilderContext<TRequest, TResponse> context)
        {
            _context = context;
            _proxyGeneratorProvider = new SingletonProxyGeneratorProvider();
            _clientInterceptorFactory = new ClientInterceptorFactory(_proxyGeneratorProvider.Value);
            _clientGenerator = new ClientGenerator(_proxyGeneratorProvider.Value);
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> EnsuringCustomSuccess(
            IEnsuringSettings<TRequest, TResponse> ensuringSettings)
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithEnsuringSetting(ensuringSettings));
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> EnsuringCustomSuccess(
            Predicate<IResponseContext<TRequest, TResponse>> successCondition, Action<IResponseContext<TRequest, TResponse>> onFailure)
        {
            Ensure.IsNotNull(successCondition, nameof(successCondition));
            Ensure.IsNotNull(onFailure, nameof(onFailure));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithEnsuringSetting(successCondition, onFailure));
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> NotEnsuringSuccess()
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithoutEnsuringSetting());
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomSerialization(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithSerializer(serializerProvider));
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomHandling(params IClientHandler<TRequest, TResponse>[] handlers)
        {
            Ensure.IsNotNull(handlers, nameof(handlers));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithHandlers(handlers));
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutHandling()
        {
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithoutHandlers());
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithForceResilience(IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            Ensure.IsNotNull(provider, nameof(provider));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithResiliencePolicy(new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(provider)));
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithIdempotentResilience(
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(idempotentMethodProvider, nameof(idempotentMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithResiliencePolicy(new IdempotentMethodResiliencePolicyProvider<TRequest, TResponse>(idempotentMethodProvider, otherMethodProvider)));
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithSafeResilience(
            IResiliencePolicyProvider<TRequest, TResponse> safeMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(safeMethodProvider, nameof(safeMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithResiliencePolicy(new SafeMethodResiliencePolicyProvider<TRequest, TResponse>(safeMethodProvider, otherMethodProvider)));
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResilience(IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider)
        {
            Ensure.IsNotNull(methodResiliencePolicyProvider, nameof(methodResiliencePolicyProvider));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithResiliencePolicy(methodResiliencePolicyProvider));
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResilience(Action<INClientResilienceMethodSelector<TClient, TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModificator = new BuilderContextModificator<TRequest, TResponse>();
            configure(new NClientResilienceMethodSelector<TClient, TRequest, TResponse>(builderContextModificator));
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(builderContextModificator.Invoke(_context));
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
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILogger<TClient> logger)
        {
            Ensure.IsNotNull(logger, nameof(logger));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithLogging(logger));
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILogger logger)
        {
            Ensure.IsNotNull(logger, nameof(logger));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithLogging(logger));
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
                new Uri(_context.Host),
                _context.HttpClientProvider,
                _context.HttpMessageBuilderProvider,
                _context.SerializerProvider,
                _context.ClientHandlers.ToArray(),
                new ResponseValidator<TRequest, TResponse>(_context.EnsuringSettings ?? new StubEnsuringSettings<TRequest, TResponse>()),
                _context.MethodResiliencePolicyProvider
                ?? new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(
                    _context.AllMethodsResiliencePolicyProvider ?? new StubResiliencePolicyProvider<TRequest, TResponse>(), 
                    _context.MethodsWithResiliencePolicy),
                _context.Logger as ILogger<TClient> ?? _context.LoggerFactory?.CreateLogger<TClient>());

            return _clientGenerator.CreateClient<TClient>(interceptor);
        }
    }
}
