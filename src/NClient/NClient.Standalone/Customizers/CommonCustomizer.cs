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
    internal abstract class CommonCustomizer<TSpecificCustomizer, TResult, TRequest, TResponse>
        : INClientCommonCustomizer<TSpecificCustomizer, TResult, TRequest, TResponse>
        where TSpecificCustomizer : class, INClientCommonCustomizer<TSpecificCustomizer, TResult, TRequest, TResponse>
    {
        private readonly ConcurrentDictionary<MethodInfo, IResiliencePolicyProvider<TResponse>> _specificResiliencePolicyProviders;
        private IMethodResiliencePolicyProvider<TResponse> _methodResiliencePolicyProvider;

        protected IHttpClientProvider<TRequest, TResponse> HttpClientProvider;
        protected IHttpMessageBuilderProvider<TRequest, TResponse> HttpMessageBuilderProvider;
        protected IHttpClientExceptionFactory<TRequest, TResponse> HttpClientExceptionFactory;
        protected ISerializerProvider SerializerProvider;
        protected IReadOnlyCollection<IClientHandler<TRequest, TResponse>> ClientHandlers;
        protected ILoggerFactory? LoggerFactory;
        protected ILogger<TResult>? Logger;

        protected CommonCustomizer(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory,
            IMethodResiliencePolicyProvider<TResponse> methodResiliencePolicyProvider,
            ISerializerProvider serializerProvider)
        {
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _specificResiliencePolicyProviders = new ConcurrentDictionary<MethodInfo, IResiliencePolicyProvider<TResponse>>();
            
            HttpClientProvider = httpClientProvider;
            HttpMessageBuilderProvider = httpMessageBuilderProvider;
            HttpClientExceptionFactory = httpClientExceptionFactory;
            SerializerProvider = serializerProvider;
            ClientHandlers = CreateClientHandlerCollection();
        }

        public TSpecificCustomizer WithCustomHttpClient(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider, 
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(httpMessageBuilderProvider, nameof(httpMessageBuilderProvider));
            Ensure.IsNotNull(httpClientExceptionFactory, nameof(httpClientExceptionFactory));

            Interlocked.Exchange(ref HttpClientProvider, httpClientProvider);
            Interlocked.Exchange(ref HttpMessageBuilderProvider, httpMessageBuilderProvider);
            Interlocked.Exchange(ref HttpClientExceptionFactory, httpClientExceptionFactory);
            return (this as TSpecificCustomizer)!;
        }
        
        public TSpecificCustomizer WithCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            Interlocked.Exchange(ref SerializerProvider, serializerProvider);
            return (this as TSpecificCustomizer)!;
        }

        public TSpecificCustomizer WithCustomHandlers(IReadOnlyCollection<IClientHandler<TRequest, TResponse>> handlers)
        {
            Ensure.IsNotNull(handlers, nameof(handlers));

            Interlocked.Exchange(ref ClientHandlers, CreateClientHandlerCollection(handlers));
            return (this as TSpecificCustomizer)!;
        }

        public TSpecificCustomizer WithResiliencePolicy(IResiliencePolicyProvider<TResponse> resiliencePolicyProvider)
        {
            Ensure.IsNotNull(resiliencePolicyProvider, nameof(resiliencePolicyProvider));

            Interlocked.Exchange(ref _methodResiliencePolicyProvider, new DefaultMethodResiliencePolicyProvider<TResponse>(resiliencePolicyProvider));
            return (this as TSpecificCustomizer)!;
        }

        public TSpecificCustomizer WithResiliencePolicy(IMethodResiliencePolicyProvider<TResponse> methodResiliencePolicyProvider)
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

        protected IMethodResiliencePolicyProvider<TResponse> CreateMethodResiliencePolicyProvider()
        {
            return new DefaultMethodResiliencePolicyProvider<TResponse>(
                _methodResiliencePolicyProvider,
                _specificResiliencePolicyProviders);
        }

        protected void AddSpecificResiliencePolicyProvider<TInterface>(
            Expression<Func<TInterface, Delegate>> methodSelector, IResiliencePolicyProvider<TResponse> resiliencePolicyProvider)
        {
            var func = methodSelector.Compile();
            var methodInfo = func.Invoke(default!).Method;
            _specificResiliencePolicyProviders[methodInfo] = resiliencePolicyProvider;
        }

        private static IReadOnlyCollection<IClientHandler<TRequest, TResponse>> CreateClientHandlerCollection(
            IReadOnlyCollection<IClientHandler<TRequest, TResponse>>? customClientHandlers = null)
        {
            var clientHandlerCollection = new List<IClientHandler<TRequest, TResponse>>(customClientHandlers ?? Array.Empty<IClientHandler<TRequest, TResponse>>());
            return clientHandlerCollection;
        }
    }
}
