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

namespace NClient.Customizers
{
    internal abstract class CommonCustomizer<TSpecificCustomizer, TResult>
        : INClientCommonCustomizer<TSpecificCustomizer, TResult>
        where TSpecificCustomizer : class, INClientCommonCustomizer<TSpecificCustomizer, TResult>
    {
        private readonly ConcurrentDictionary<MethodInfo, IResiliencePolicyProvider> _specificResiliencePolicyProviders;
        private IMethodResiliencePolicyProvider? _methodResiliencePolicyProvider;

        protected IHttpClientProvider HttpClientProvider;
        protected ISerializerProvider SerializerProvider;
        protected IReadOnlyCollection<IClientHandler> ClientHandlers;
        protected ILoggerFactory? LoggerFactory;
        protected ILogger<TResult>? Logger;

        protected CommonCustomizer(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider)
        {
            _specificResiliencePolicyProviders = new ConcurrentDictionary<MethodInfo, IResiliencePolicyProvider>();

            HttpClientProvider = httpClientProvider;
            SerializerProvider = serializerProvider;
            ClientHandlers = CreateClientHandlerCollection();
        }

        public TSpecificCustomizer WithCustomHttpClient(IHttpClientProvider httpClientProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));

            Interlocked.Exchange(ref HttpClientProvider, httpClientProvider);
            return (this as TSpecificCustomizer)!;
        }

        public TSpecificCustomizer WithCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            Interlocked.Exchange(ref SerializerProvider, serializerProvider);
            return (this as TSpecificCustomizer)!;
        }

        public TSpecificCustomizer WithCustomHandlers(IReadOnlyCollection<IClientHandler> handlers)
        {
            Ensure.IsNotNull(handlers, nameof(handlers));

            Interlocked.Exchange(ref ClientHandlers, CreateClientHandlerCollection(handlers));
            return (this as TSpecificCustomizer)!;
        }

        public TSpecificCustomizer WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            Ensure.IsNotNull(resiliencePolicyProvider, nameof(resiliencePolicyProvider));

            Interlocked.Exchange(ref _methodResiliencePolicyProvider, new DefaultMethodResiliencePolicyProvider(resiliencePolicyProvider));
            return (this as TSpecificCustomizer)!;
        }

        public TSpecificCustomizer WithResiliencePolicy(IMethodResiliencePolicyProvider methodResiliencePolicyProvider)
        {
            Ensure.IsNotNull(methodResiliencePolicyProvider, nameof(methodResiliencePolicyProvider));

            Interlocked.Exchange(ref _methodResiliencePolicyProvider, methodResiliencePolicyProvider);
            return (this as TSpecificCustomizer)!;
        }

        public TSpecificCustomizer WithLogging(ILoggerFactory loggerFactory)
        {
            Ensure.IsNotNull(loggerFactory, nameof(loggerFactory));

            Interlocked.Exchange(ref LoggerFactory, loggerFactory);
            Interlocked.Exchange(ref Logger, loggerFactory.CreateLogger<TResult>());
            return (this as TSpecificCustomizer)!;
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
