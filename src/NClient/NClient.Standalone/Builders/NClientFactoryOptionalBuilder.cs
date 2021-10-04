using System;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.Builders;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Handling;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Resilience.Providers;
using NClient.Abstractions.Serialization;
using NClient.Builders.Context;
using NClient.ClientGeneration;
using NClient.Common.Helpers;
using NClient.Configuration.Resilience;
using NClient.Core.Interceptors;
using NClient.Core.Proxy;

namespace NClient.Builders
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
            _context.SetEnsuringSetting(ensuringSettings);
            return this;
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> EnsuringCustomSuccess(
            Predicate<ResponseContext<TRequest, TResponse>> successCondition, Action<ResponseContext<TRequest, TResponse>> onFailure)
        {
            Ensure.IsNotNull(successCondition, nameof(successCondition));
            Ensure.IsNotNull(onFailure, nameof(onFailure));
            _context.SetEnsuringSetting(successCondition, onFailure);
            return this;
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> NotEnsuringSuccess()
        {
            _context.ClearEnsuringSetting();
            return this;
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithSerializerReplacedBy(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            _context.SetSerializer(serializerProvider);
            return this;
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomHandling(params IClientHandler<TRequest, TResponse>[] handlers)
        {
            Ensure.IsNotNull(handlers, nameof(handlers));
            _context.SetHandlers(handlers);
            return this;
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutHandling()
        {
            _context.ClearHandlers();
            return this;
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithForceResilience(IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            Ensure.IsNotNull(provider, nameof(provider));
            _context.SetResiliencePolicy(new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(provider));
            return this;
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithIdempotentResilience(
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(idempotentMethodProvider, nameof(idempotentMethodProvider));
            _context.SetResiliencePolicy(new IdempotentMethodResiliencePolicyProvider<TRequest, TResponse>(idempotentMethodProvider, otherMethodProvider));
            return this;
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithSafeResilience(
            IResiliencePolicyProvider<TRequest, TResponse> safeMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(safeMethodProvider, nameof(safeMethodProvider));
            _context.SetResiliencePolicy(new SafeMethodResiliencePolicyProvider<TRequest, TResponse>(safeMethodProvider, otherMethodProvider));
            return this;
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResilience(IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider)
        {
            Ensure.IsNotNull(methodResiliencePolicyProvider, nameof(methodResiliencePolicyProvider));
            _context.SetResiliencePolicy(methodResiliencePolicyProvider);
            return this;
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResilience(Action<INClientFactoryResilienceMethodSelector<TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));
            configure(new NClientFactoryResilienceMethodSelector<TRequest, TResponse>(_context));
            return this;
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutResilience()
        {
            _context.ClearResiliencePolicy();
            return this;
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory)
        {
            Ensure.IsNotNull(loggerFactory, nameof(loggerFactory));
            _context.SetLogging(loggerFactory);
            return this;
        }

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithLogging(ILogger logger)
        {
            Ensure.IsNotNull(logger, nameof(logger));
            _context.SetLogging(logger);
            return this;
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutLogging()
        {
            _context.ClearLogging();
            return this;
        }

        public INClientFactory Build()
        {
            return new CustomNClientFactory<TRequest, TResponse>(_factoryName, _context);
        }
    }
}
