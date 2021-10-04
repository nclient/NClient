using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Handling;
using NClient.Abstractions.Helpers;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Core.Resilience;
using NClient.Exceptions.Factories;

namespace NClient.Builders.Context
{
    internal class CustomizerContext<TRequest, TResponse>
    {
        private readonly IClientBuildExceptionFactory _clientBuildExceptionFactory;
        
        public string Host { get; private set; } = null!;

        public IHttpClientProvider<TRequest, TResponse> HttpClientProvider { get; private set; } = null!;
        public IHttpMessageBuilderProvider<TRequest, TResponse> HttpMessageBuilderProvider { get; private set; } = null!;
        
        public ISerializerProvider SerializerProvider { get; private set; } = null!;

        public IEnsuringSettings<TRequest, TResponse>? EnsuringSettings { get; private set; }

        public ICollection<IClientHandler<TRequest, TResponse>> ClientHandlers { get; private set; }

        public IMethodResiliencePolicyProvider<TRequest, TResponse>? MethodResiliencePolicyProvider { get; private set; }
        public IResiliencePolicyProvider<TRequest, TResponse>? AllMethodsResiliencePolicyProvider { get; private set; }
        public Dictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>> MethodsWithResiliencePolicy { get; private set; }

        public ILogger? Logger { get; private set; }
        public ILoggerFactory? LoggerFactory { get; private set; }

        public CustomizerContext()
        {
            ClientHandlers = new List<IClientHandler<TRequest, TResponse>>();
            MethodsWithResiliencePolicy = new Dictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>(new MethodInfoEqualityComparer());
            _clientBuildExceptionFactory = new ClientBuildExceptionFactory();
        }

        public void SetHost(string host)
        {
            Host = host;
        }

        public void SetHttpClientProvider(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider)
        {
            HttpClientProvider = httpClientProvider;
            HttpMessageBuilderProvider = httpMessageBuilderProvider;
        }

        public void SetSerializer(ISerializerProvider serializerProvider)
        {
            SerializerProvider = serializerProvider;
        }
        
        public void SetEnsuringSetting(
            Predicate<ResponseContext<TRequest, TResponse>> successCondition, Action<ResponseContext<TRequest, TResponse>> onFailure)
        {
            SetEnsuringSetting(new EnsuringSettings<TRequest, TResponse>(successCondition, onFailure));
        }
        
        public void SetEnsuringSetting(IEnsuringSettings<TRequest, TResponse> ensuringSettings)
        {
            EnsuringSettings = ensuringSettings;
        }
        
        public void ClearEnsuringSetting()
        {
            EnsuringSettings = null;
        }

        public void SetHandlers(IEnumerable<IClientHandler<TRequest, TResponse>> clientHandlers)
        {
            foreach (var clientHandler in clientHandlers)
            {
                ClientHandlers.Add(clientHandler);
            }
        }

        public void ClearHandlers()
        {
            ClientHandlers.Clear();
        }

        public void SetResiliencePolicy(IMethodResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            MethodResiliencePolicyProvider = provider;
        }
        
        public void SetResiliencePolicy(IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            AllMethodsResiliencePolicyProvider = provider;
            MethodsWithResiliencePolicy.Clear();
        }

        public void SetResiliencePolicy(MethodInfo methodInfo, IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            MethodsWithResiliencePolicy[methodInfo] = provider;
        }
        
        public void SetResiliencePolicy(IEnumerable<MethodInfo> methodInfos, IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            foreach (var methodInfo in methodInfos)
            {
                SetResiliencePolicy(methodInfo, provider);
            }
        }
        
        public void ClearAllMethodsResiliencePolicy()
        {
            AllMethodsResiliencePolicyProvider = null;
            MethodsWithResiliencePolicy.Clear();
        }
        
        public void ClearMethodResiliencePolicy(MethodInfo methodInfo)
        {
            MethodsWithResiliencePolicy[methodInfo] = new StubResiliencePolicyProvider<TRequest, TResponse>();
        }
        
        public void ClearMethodResiliencePolicy(IEnumerable<MethodInfo> methodInfos)
        {
            foreach (var methodInfo in methodInfos)
            {
                ClearMethodResiliencePolicy(methodInfo);
            }
        }

        public void ClearResiliencePolicy()
        {
            MethodResiliencePolicyProvider = null;
            AllMethodsResiliencePolicyProvider = null;
            MethodsWithResiliencePolicy.Clear();
        }

        public void SetLogging(ILogger logger)
        {
            Logger = logger;
        }
        
        public void SetLogging(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
        }

        public void ClearLogging()
        {
            Logger = null;
            LoggerFactory = null;
        }

        public CustomizerContext<TRequest, TResponse> EnsureComplete()
        {
            if (Host is null) 
                throw _clientBuildExceptionFactory.HostIsNotSet();
            if (HttpClientProvider is null || HttpMessageBuilderProvider is null)
                throw _clientBuildExceptionFactory.HttpClientIsNotSet();
            if (SerializerProvider is null)
                throw _clientBuildExceptionFactory.SerializerIsNotSet();
            
            return this;
        }
    }
}
