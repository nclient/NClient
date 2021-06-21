using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Core.Resilience;

namespace NClient.OptionalNClientBuilders.Bases
{
    internal abstract class OptionalNClientBuilderBase<TBuilder, TResult>
        : IOptionalBuilderBase<TBuilder, TResult>
        where TBuilder : class, IOptionalBuilderBase<TBuilder, TResult>
    {
        private readonly ConcurrentDictionary<MethodInfo, IResiliencePolicyProvider> _specificResiliencePolicyProviders;
        private IMethodResiliencePolicyProvider? _methodResiliencePolicyProvider;

        protected IHttpClientProvider HttpClientProvider;
        protected ISerializerProvider SerializerProvider;
        protected IReadOnlyCollection<IClientHandler> ClientHandlers;
        protected ILoggerFactory? LoggerFactory;
        protected ILogger<TResult>? Logger;

        protected OptionalNClientBuilderBase(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider)
        {
            _specificResiliencePolicyProviders = new ConcurrentDictionary<MethodInfo, IResiliencePolicyProvider>();

            HttpClientProvider = httpClientProvider;
            SerializerProvider = serializerProvider;
            ClientHandlers = CreateClientHandlerCollection();
        }

        public TBuilder WithCustomHttpClient(IHttpClientProvider httpClientProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));

            Interlocked.Exchange(ref HttpClientProvider, httpClientProvider);
            return (this as TBuilder)!;
        }

        public TBuilder WithCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            Interlocked.Exchange(ref SerializerProvider, serializerProvider);
            return (this as TBuilder)!;
        }

        public TBuilder WithCustomHandlers(IReadOnlyCollection<IClientHandler> handlers)
        {
            Ensure.IsNotNull(handlers, nameof(handlers));

            Interlocked.Exchange(ref ClientHandlers, CreateClientHandlerCollection(handlers));
            return (this as TBuilder)!;
        }

        public TBuilder WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            Ensure.IsNotNull(resiliencePolicyProvider, nameof(resiliencePolicyProvider));

            Interlocked.Exchange(ref _methodResiliencePolicyProvider, new DefaultMethodResiliencePolicyProvider(resiliencePolicyProvider));
            return (this as TBuilder)!;
        }

        public TBuilder WithResiliencePolicy(IMethodResiliencePolicyProvider methodResiliencePolicyProvider)
        {
            Ensure.IsNotNull(methodResiliencePolicyProvider, nameof(methodResiliencePolicyProvider));

            Interlocked.Exchange(ref _methodResiliencePolicyProvider, methodResiliencePolicyProvider);
            return (this as TBuilder)!;
        }

        public TBuilder WithLogging(ILoggerFactory loggerFactory)
        {
            Ensure.IsNotNull(loggerFactory, nameof(loggerFactory));

            Interlocked.Exchange(ref LoggerFactory, loggerFactory);
            Interlocked.Exchange(ref Logger, loggerFactory.CreateLogger<TResult>());
            return (this as TBuilder)!;
        }

        public TBuilder WithLogging(ILogger<TResult> logger)
        {
            Ensure.IsNotNull(logger, nameof(logger));

            Interlocked.Exchange(ref Logger, logger);
            return (this as TBuilder)!;
        }

        public abstract TResult Build();

        protected IMethodResiliencePolicyProvider GetOrCreateMethodResiliencePolicyProvider()
        {
            return _methodResiliencePolicyProvider is not null
                ? new DefaultMethodResiliencePolicyProvider(
                    _methodResiliencePolicyProvider,
                    _specificResiliencePolicyProviders)
                : new DefaultMethodResiliencePolicyProvider(
                    new DefaultResiliencePolicyProvider(),
                    _specificResiliencePolicyProviders);
        }

        protected void AddSpecificResiliencePolicyProvider<TInterface>(
            Expression<Func<TInterface, Delegate>> methodSelector, IResiliencePolicyProvider resiliencePolicyProvider)
        {
            var func = methodSelector.Compile();
            var methodInfo = func.Invoke(default!).Method;
            _specificResiliencePolicyProviders[methodInfo] = resiliencePolicyProvider;
        }

        private static IReadOnlyCollection<IClientHandler> CreateClientHandlerCollection(
            IReadOnlyCollection<IClientHandler>? customClientHandlers = null)
        {
            var clientHandlerCollection = new List<IClientHandler>(customClientHandlers ?? Array.Empty<IClientHandler>());
            return clientHandlerCollection;
        }
    }
}