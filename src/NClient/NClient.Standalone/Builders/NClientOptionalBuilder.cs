using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Builders;
using NClient.Abstractions.Customization.Resilience;
using NClient.Abstractions.Handling;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Resilience.Providers;
using NClient.Abstractions.Serialization;
using NClient.Builders.Context;
using NClient.ClientGeneration;
using NClient.Common.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.Proxy;
using NClient.Core.Validation;
using NClient.Customization.Resilience;

namespace NClient.Builders
{
    internal class NClientOptionalBuilder<TClient, TRequest, TResponse> : INClientOptionalBuilder<TClient, TRequest, TResponse>
        where TClient : class
    {
        private readonly CustomizerContext<TRequest, TResponse> _context;
        private readonly IClientInterceptorFactory _clientInterceptorFactory;
        private readonly IClientGenerator _clientGenerator;

        public NClientOptionalBuilder(CustomizerContext<TRequest, TResponse> context)
        {
            _context = context;
            var proxyGeneratorProvider = new SingletonProxyGeneratorProvider();
            _clientInterceptorFactory = new ClientInterceptorFactory(proxyGeneratorProvider.Value);
            new ClientValidator(proxyGeneratorProvider.Value).EnsureAsync<TClient>(_clientInterceptorFactory);
            _clientGenerator = new ClientGenerator(proxyGeneratorProvider.Value);
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> ChangeSerializerToCustom(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            _context.SetSerializer(serializerProvider);
            return this;
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomHandling(IReadOnlyCollection<IClientHandler<TRequest, TResponse>> handlers)
        {
            Ensure.IsNotNull(handlers, nameof(handlers));
            _context.SetHandlers(handlers);
            return this;
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutHandling()
        {
            _context.ClearHandlers();
            return this;
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithForceResilience(IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            Ensure.IsNotNull(provider, nameof(provider));
            _context.SetResiliencePolicy(new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(provider));
            return this;
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithIdempotentResilience(
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(idempotentMethodProvider, nameof(idempotentMethodProvider));
            _context.SetResiliencePolicy(new IdempotentMethodResiliencePolicyProvider<TRequest, TResponse>(idempotentMethodProvider, otherMethodProvider));
            return this;
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithSafeResilience(
            IResiliencePolicyProvider<TRequest, TResponse> safeMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(safeMethodProvider, nameof(safeMethodProvider));
            _context.SetResiliencePolicy(new SafeMethodResiliencePolicyProvider<TRequest, TResponse>(safeMethodProvider, otherMethodProvider));
            return this;
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResilience(IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider)
        {
            Ensure.IsNotNull(methodResiliencePolicyProvider, nameof(methodResiliencePolicyProvider));
            _context.SetResiliencePolicy(methodResiliencePolicyProvider);
            return this;
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResilience(Action<INClientResilienceMethodSelector<TClient, TRequest, TResponse>> customizer)
        {
            Ensure.IsNotNull(customizer, nameof(customizer));
            customizer(new NClientResilienceMethodSelector<TClient, TRequest, TResponse>(_context));
            return this;
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResilience()
        {
            _context.ClearResiliencePolicy();
            return this;
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory)
        {
            Ensure.IsNotNull(loggerFactory, nameof(loggerFactory));
            _context.SetLogging(loggerFactory);
            return this;
        }
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILogger<TClient> logger)
        {
            Ensure.IsNotNull(logger, nameof(logger));
            _context.SetLogging(logger);
            return this;
        }

        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILogger logger)
        {
            Ensure.IsNotNull(logger, nameof(logger));
            _context.SetLogging(logger);
            return this;
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutLogging()
        {
            _context.ClearLogging();
            return this;
        }

        public TClient Build()
        {
            _context.EnsureComplete();
            
            var interceptor = _clientInterceptorFactory.Create(
                new Uri(_context.Host),
                _context.HttpClientProvider,
                _context.HttpMessageBuilderProvider,
                _context.HttpClientExceptionFactory,
                _context.SerializerProvider,
                _context.ClientHandlers.ToArray(),
                _context.MethodResiliencePolicyProvider
                ?? new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(
                    _context.AllMethodsResiliencePolicyProvider ?? new NoResiliencePolicyProvider<TRequest, TResponse>(), 
                    _context.MethodsWithResiliencePolicy),
                _context.Logger as ILogger<TClient>);

            return _clientGenerator.CreateClient<TClient>(interceptor);
        }
    }
}
