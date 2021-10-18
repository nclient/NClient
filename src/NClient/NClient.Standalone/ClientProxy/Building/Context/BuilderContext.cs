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
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Building.Context
{
    // TODO: Move to separate class
    internal class ResiliencePolicyPredicate<TRequest, TResponse>
    {
        public IResiliencePolicyProvider<TRequest, TResponse> Provider { get; }
        public Func<MethodInfo, IHttpRequest, bool> Predicate { get; }
        
        public ResiliencePolicyPredicate(IResiliencePolicyProvider<TRequest, TResponse> provider, Func<MethodInfo, IHttpRequest, bool> predicate)
        {
            Provider = provider;
            Predicate = predicate;
        }
    }
    
    internal class BuilderContext<TRequest, TResponse>
    {
        private readonly IClientBuildExceptionFactory _clientBuildExceptionFactory;
        
        public string Host { get; private set; } = null!;

        public IHttpClientProvider<TRequest, TResponse> HttpClientProvider { get; private set; } = null!;
        public IHttpMessageBuilderProvider<TRequest, TResponse> HttpMessageBuilderProvider { get; private set; } = null!;
        
        public ISerializerProvider SerializerProvider { get; private set; } = null!;

        public IReadOnlyCollection<IEnsuringSettings<TRequest, TResponse>> EnsuringSettings { get; private set; }

        public IReadOnlyCollection<IClientHandler<TRequest, TResponse>> ClientHandlers { get; private set; }

        public IMethodResiliencePolicyProvider<TRequest, TResponse>? AllMethodsResiliencePolicyProvider { get; private set; }
        public IReadOnlyCollection<ResiliencePolicyPredicate<TRequest, TResponse>> MethodsWithResiliencePolicy { get; private set; }
        
        public IReadOnlyCollection<IResultBuilderProvider<IHttpResponse>> ResultBuilderProviders { get; private set; }
        public IReadOnlyCollection<IResultBuilderProvider<TResponse>> TypedResultBuilderProviders { get; private set; }

        public IReadOnlyCollection<ILogger> Loggers { get; private set; }
        public ILoggerFactory? LoggerFactory { get; private set; }

        public BuilderContext()
        {
            EnsuringSettings = Array.Empty<IEnsuringSettings<TRequest, TResponse>>();
            ClientHandlers = Array.Empty<IClientHandler<TRequest, TResponse>>();
            MethodsWithResiliencePolicy = Array.Empty<ResiliencePolicyPredicate<TRequest, TResponse>>();
            ResultBuilderProviders = Array.Empty<IResultBuilderProvider<IHttpResponse>>();
            TypedResultBuilderProviders = Array.Empty<IResultBuilderProvider<TResponse>>();
            Loggers = Array.Empty<ILogger>();
            _clientBuildExceptionFactory = new ClientBuildExceptionFactory();
        }

        public BuilderContext(BuilderContext<TRequest, TResponse> builderContext)
        {
            _clientBuildExceptionFactory = builderContext._clientBuildExceptionFactory;
            
            Host = builderContext.Host;
            
            HttpClientProvider = builderContext.HttpClientProvider;
            HttpMessageBuilderProvider = builderContext.HttpMessageBuilderProvider;

            SerializerProvider = builderContext.SerializerProvider;

            EnsuringSettings = builderContext.EnsuringSettings.ToArray();

            ClientHandlers = builderContext.ClientHandlers.ToArray();

            AllMethodsResiliencePolicyProvider = builderContext.AllMethodsResiliencePolicyProvider;
            MethodsWithResiliencePolicy = builderContext.MethodsWithResiliencePolicy.ToArray();

            ResultBuilderProviders = builderContext.ResultBuilderProviders.ToArray();
            TypedResultBuilderProviders = builderContext.TypedResultBuilderProviders.ToArray();
            
            Loggers = builderContext.Loggers.ToArray();
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
            return WithEnsuringSetting(new[] { new EnsuringSettings<TRequest, TResponse>(successCondition, onFailure) });
        }
        
        public BuilderContext<TRequest, TResponse> WithEnsuringSetting(IEnumerable<IEnsuringSettings<TRequest, TResponse>> ensuringSettings)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                EnsuringSettings = EnsuringSettings.Concat(ensuringSettings).ToArray()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithoutEnsuringSetting()
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                EnsuringSettings = Array.Empty<IEnsuringSettings<TRequest, TResponse>>()
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
                ClientHandlers = Array.Empty<IClientHandler<TRequest, TResponse>>()
            };
        }

        public BuilderContext<TRequest, TResponse> WithResiliencePolicy(IMethodResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                AllMethodsResiliencePolicyProvider = provider
            };
        }

        public BuilderContext<TRequest, TResponse> WithResiliencePolicy(Func<MethodInfo, IHttpRequest, bool> predicate, IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                MethodsWithResiliencePolicy = MethodsWithResiliencePolicy
                    .Concat(new[] { new ResiliencePolicyPredicate<TRequest, TResponse>(provider, predicate) })
                    .ToArray()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithResiliencePolicy(IEnumerable<Func<MethodInfo, IHttpRequest, bool>> predicates, IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                MethodsWithResiliencePolicy = MethodsWithResiliencePolicy
                    .Concat(predicates.Select(predicate => new ResiliencePolicyPredicate<TRequest, TResponse>(provider, predicate)))
                    .ToArray()
            };
        }

        public BuilderContext<TRequest, TResponse> WithoutResiliencePolicy()
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                AllMethodsResiliencePolicyProvider = null,
                MethodsWithResiliencePolicy = Array.Empty<ResiliencePolicyPredicate<TRequest, TResponse>>()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithResultBuilders(IEnumerable<IResultBuilderProvider<IHttpResponse>> resultBuilderProviders)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                ResultBuilderProviders = ResultBuilderProviders.Concat(resultBuilderProviders).ToArray()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithResultBuilders(IEnumerable<IResultBuilderProvider<TResponse>> resultBuilderProviders)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                TypedResultBuilderProviders = TypedResultBuilderProviders.Concat(resultBuilderProviders).ToArray()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithoutResultBuilders()
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                ResultBuilderProviders = Array.Empty<IResultBuilderProvider<IHttpResponse>>(),
                TypedResultBuilderProviders = Array.Empty<IResultBuilderProvider<TResponse>>()
            };
        }

        public BuilderContext<TRequest, TResponse> WithLogging(IEnumerable<ILogger> loggers)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                Loggers = Loggers.Concat(loggers).ToArray()
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
                Loggers = Array.Empty<ILogger>(),
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
