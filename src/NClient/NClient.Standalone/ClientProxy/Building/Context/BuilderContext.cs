using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Results;
using NClient.Abstractions.Serialization;
using NClient.Core.Helpers;
using NClient.Standalone.Client.Resilience;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Building.Context
{
    internal class BuilderContext<TRequest, TResponse>
    {
        private readonly IClientBuildExceptionFactory _clientBuildExceptionFactory;
        
        public string Host { get; private set; } = null!;

        public IHttpClientProvider<TRequest, TResponse> HttpClientProvider { get; private set; } = null!;
        public IHttpMessageBuilderProvider<TRequest, TResponse> HttpMessageBuilderProvider { get; private set; } = null!;
        
        public ISerializerProvider SerializerProvider { get; private set; } = null!;

        public IEnsuringSettings<TRequest, TResponse>? EnsuringSettings { get; private set; }

        public IReadOnlyCollection<IClientHandler<TRequest, TResponse>> ClientHandlers { get; private set; }

        public IMethodResiliencePolicyProvider<TRequest, TResponse>? MethodResiliencePolicyProvider { get; private set; }
        public IResiliencePolicyProvider<TRequest, TResponse>? AllMethodsResiliencePolicyProvider { get; private set; }
        public IReadOnlyDictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>> MethodsWithResiliencePolicy { get; private set; }
        
        public IReadOnlyCollection<IResultBuilderProvider<IHttpResponse>> ResultBuilderProviders { get; private set; }

        public ILogger? Logger { get; private set; }
        public ILoggerFactory? LoggerFactory { get; private set; }

        public BuilderContext()
        {
            ClientHandlers = Array.Empty<IClientHandler<TRequest, TResponse>>();
            MethodsWithResiliencePolicy = new Dictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>(new MethodInfoEqualityComparer());
            ResultBuilderProviders = Array.Empty<IResultBuilderProvider<IHttpResponse>>();
            _clientBuildExceptionFactory = new ClientBuildExceptionFactory();
        }

        public BuilderContext(BuilderContext<TRequest, TResponse> builderContext)
        {
            _clientBuildExceptionFactory = builderContext._clientBuildExceptionFactory;
            
            Host = builderContext.Host;
            
            HttpClientProvider = builderContext.HttpClientProvider;
            HttpMessageBuilderProvider = builderContext.HttpMessageBuilderProvider;

            SerializerProvider = builderContext.SerializerProvider;

            EnsuringSettings = builderContext.EnsuringSettings;

            ClientHandlers = builderContext.ClientHandlers.ToList();

            MethodResiliencePolicyProvider = builderContext.MethodResiliencePolicyProvider;
            AllMethodsResiliencePolicyProvider = builderContext.AllMethodsResiliencePolicyProvider;
            MethodsWithResiliencePolicy = builderContext.MethodsWithResiliencePolicy.ToDictionary(x => x.Key, x => x.Value);

            ResultBuilderProviders = builderContext.ResultBuilderProviders;
            
            Logger = builderContext.Logger;
            LoggerFactory = builderContext.LoggerFactory;
        }

        public BuilderContext<TRequest, TResponse> WithHost(string host)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                Host = host
            };
        }

        public BuilderContext<TRequest, TResponse> WithHttpClientProvider(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                HttpClientProvider = httpClientProvider,
                HttpMessageBuilderProvider = httpMessageBuilderProvider
            };
        }

        public BuilderContext<TRequest, TResponse> WithSerializer(ISerializerProvider serializerProvider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                SerializerProvider = serializerProvider
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithEnsuringSetting(
            Predicate<IResponseContext<TRequest, TResponse>> successCondition, Action<IResponseContext<TRequest, TResponse>> onFailure)
        {
            return WithEnsuringSetting(new EnsuringSettings<TRequest, TResponse>(successCondition, onFailure));
        }
        
        public BuilderContext<TRequest, TResponse> WithEnsuringSetting(IEnsuringSettings<TRequest, TResponse> ensuringSettings)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                EnsuringSettings = ensuringSettings
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithoutEnsuringSetting()
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                EnsuringSettings = null
            };
        }

        public BuilderContext<TRequest, TResponse> WithHandlers(IEnumerable<IClientHandler<TRequest, TResponse>> clientHandlers)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                ClientHandlers = ClientHandlers.Concat(clientHandlers).ToList()
            };
        }

        public BuilderContext<TRequest, TResponse> WithoutHandlers()
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                ClientHandlers = new List<IClientHandler<TRequest, TResponse>>()
            };
        }

        public BuilderContext<TRequest, TResponse> WithResiliencePolicy(IMethodResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                MethodResiliencePolicyProvider = provider
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithResiliencePolicy(IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                AllMethodsResiliencePolicyProvider = provider,
                MethodsWithResiliencePolicy = new Dictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>()
            };
        }

        public BuilderContext<TRequest, TResponse> WithResiliencePolicy(MethodInfo methodInfo, IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                MethodsWithResiliencePolicy = new Dictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>(MethodsWithResiliencePolicy.ToDictionary(x => x.Key, x => x.Value))
                {
                    [methodInfo] = provider
                }
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithResiliencePolicy(IEnumerable<MethodInfo> methodInfos, IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                MethodsWithResiliencePolicy = MethodsWithResiliencePolicy.Concat(methodInfos.ToDictionary(x => x, _ => provider))
                    .GroupBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.Last().Value)
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithoutAllMethodsResiliencePolicy()
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                AllMethodsResiliencePolicyProvider = null,
                MethodsWithResiliencePolicy = new Dictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithoutMethodResiliencePolicy(MethodInfo methodInfo)
        {
            return WithResiliencePolicy(methodInfo, new StubResiliencePolicyProvider<TRequest, TResponse>());
        }
        
        public BuilderContext<TRequest, TResponse> WithoutMethodResiliencePolicy(IEnumerable<MethodInfo> methodInfos)
        {
            return WithResiliencePolicy(methodInfos, new StubResiliencePolicyProvider<TRequest, TResponse>());
        }

        public BuilderContext<TRequest, TResponse> WithoutResiliencePolicy()
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                MethodResiliencePolicyProvider = null,
                AllMethodsResiliencePolicyProvider = null,
                MethodsWithResiliencePolicy = new Dictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithResultBuilders(IEnumerable<IResultBuilderProvider<IHttpResponse>> resultBuilderProviders)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                ResultBuilderProviders = resultBuilderProviders.ToArray()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithoutResultBuilders()
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                ResultBuilderProviders = Array.Empty<IResultBuilderProvider<IHttpResponse>>()
            };
        }

        public BuilderContext<TRequest, TResponse> WithLogging(ILogger logger)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                Logger = logger
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                LoggerFactory = loggerFactory
            };
        }

        public BuilderContext<TRequest, TResponse> WithoutLogging()
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                Logger = null,
                LoggerFactory = null
            };
        }

        public void EnsureComplete()
        {
            if (Host is null) 
                throw _clientBuildExceptionFactory.HostIsNotSet();
            if (HttpClientProvider is null || HttpMessageBuilderProvider is null)
                throw _clientBuildExceptionFactory.HttpClientIsNotSet();
            if (SerializerProvider is null)
                throw _clientBuildExceptionFactory.SerializerIsNotSet();
        }
    }
}
