using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.Helpers;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Exceptions.Factories;

namespace NClient.Customization.Context
{
    public class CustomizerContext<TRequest, TResponse>
    {
        private readonly IClientBuildExceptionFactory _clientBuildExceptionFactory;
        
        public string Host { get; private set; } = null!;

        public IHttpClientProvider<TRequest, TResponse> HttpClientProvider { get; private set; } = null!;
        public IHttpMessageBuilderProvider<TRequest, TResponse> HttpMessageBuilderProvider { get; private set; } = null!;
        public IHttpClientExceptionFactory<TRequest, TResponse> HttpClientExceptionFactory { get; private set; } = null!;

        public ISerializerProvider SerializerProvider { get; private set; } = null!;

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
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory)
        {
            HttpClientProvider = httpClientProvider;
            HttpMessageBuilderProvider = httpMessageBuilderProvider;
            HttpClientExceptionFactory = httpClientExceptionFactory;
        }

        public void SetSerializer(ISerializerProvider serializerProvider)
        {
            SerializerProvider = serializerProvider;
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

        public void ClearResiliencePolicy()
        {
            AllMethodsResiliencePolicyProvider = null;
            MethodsWithResiliencePolicy.Clear();
        }

        public void SetLogging(ILogger logger)
        {
            Logger = logger;
        }
        
        public void SetLogging<TResult>(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger<TResult>();
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
            if (HttpClientProvider is null || HttpMessageBuilderProvider is null || HttpClientExceptionFactory is null)
                throw _clientBuildExceptionFactory.HttpClientIsNotSet();
            if (SerializerProvider is null)
                throw _clientBuildExceptionFactory.SerializerIsNotSet();
            
            return this;
        }
    }
}
