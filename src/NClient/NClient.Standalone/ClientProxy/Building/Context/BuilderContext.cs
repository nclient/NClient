using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NClient.Providers.Handling;
using NClient.Providers.Resilience;
using NClient.Providers.Results;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using NClient.Providers.Validation;
using NClient.Standalone.ClientProxy.Building.Models;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Building.Context
{
    internal class BuilderContext<TRequest, TResponse>
    {
        private readonly IClientBuildExceptionFactory _clientBuildExceptionFactory;
        
        public string Host { get; private set; } = null!;

        public ITransportProvider<TRequest, TResponse> TransportProvider { get; private set; } = null!;
        public ITransportMessageBuilderProvider<TRequest, TResponse> TransportMessageBuilderProvider { get; private set; } = null!;
        
        public ISerializerProvider SerializerProvider { get; private set; } = null!;

        public IReadOnlyCollection<IResponseValidatorProvider<TRequest, TResponse>> ResponseValidatorProviders { get; private set; }

        public IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> ClientHandlerProviders { get; private set; }

        public IMethodResiliencePolicyProvider<TRequest, TResponse>? AllMethodsResiliencePolicyProvider { get; private set; }
        public IReadOnlyCollection<ResiliencePolicyPredicatePair<TRequest, TResponse>> MethodsWithResiliencePolicy { get; private set; }
        
        public IReadOnlyCollection<IResultBuilderProvider<IResponse>> ResultBuilderProviders { get; private set; }
        public IReadOnlyCollection<IResultBuilderProvider<TResponse>> TypedResultBuilderProviders { get; private set; }

        public IReadOnlyCollection<ILogger> Loggers { get; private set; }
        public ILoggerFactory? LoggerFactory { get; private set; }

        public BuilderContext()
        {
            ResponseValidatorProviders = Array.Empty<IResponseValidatorProvider<TRequest, TResponse>>();
            ClientHandlerProviders = Array.Empty<IClientHandlerProvider<TRequest, TResponse>>();
            MethodsWithResiliencePolicy = Array.Empty<ResiliencePolicyPredicatePair<TRequest, TResponse>>();
            ResultBuilderProviders = Array.Empty<IResultBuilderProvider<IResponse>>();
            TypedResultBuilderProviders = Array.Empty<IResultBuilderProvider<TResponse>>();
            Loggers = Array.Empty<ILogger>();
            _clientBuildExceptionFactory = new ClientBuildExceptionFactory();
        }

        public BuilderContext(BuilderContext<TRequest, TResponse> builderContext)
        {
            _clientBuildExceptionFactory = builderContext._clientBuildExceptionFactory;
            
            Host = builderContext.Host;
            
            TransportProvider = builderContext.TransportProvider;
            TransportMessageBuilderProvider = builderContext.TransportMessageBuilderProvider;

            SerializerProvider = builderContext.SerializerProvider;

            ResponseValidatorProviders = builderContext.ResponseValidatorProviders.ToArray();

            ClientHandlerProviders = builderContext.ClientHandlerProviders.ToArray();

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
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportMessageBuilderProvider<TRequest, TResponse> transportMessageBuilderProvider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                TransportProvider = transportProvider,
                TransportMessageBuilderProvider = transportMessageBuilderProvider
            };
        }

        public BuilderContext<TRequest, TResponse> WithSerializer(ISerializerProvider serializerProvider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                SerializerProvider = serializerProvider
            };
        }

        public BuilderContext<TRequest, TResponse> WithResponseValidation(IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> responseValidatorProviders)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                ResponseValidatorProviders = ResponseValidatorProviders.Concat(responseValidatorProviders).ToArray()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithoutResponseValidation()
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                ResponseValidatorProviders = Array.Empty<IResponseValidatorProvider<TRequest, TResponse>>()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithHandlers(IEnumerable<IClientHandlerProvider<TRequest, TResponse>> clientHandlerProviders)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                ClientHandlerProviders = ClientHandlerProviders.Concat(clientHandlerProviders).ToList()
            };
        }

        public BuilderContext<TRequest, TResponse> WithoutHandlers()
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                ClientHandlerProviders = Array.Empty<IClientHandlerProvider<TRequest, TResponse>>()
            };
        }

        public BuilderContext<TRequest, TResponse> WithResiliencePolicy(IMethodResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                AllMethodsResiliencePolicyProvider = provider
            };
        }

        public BuilderContext<TRequest, TResponse> WithResiliencePolicy(Func<MethodInfo, IRequest, bool> predicate, IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                MethodsWithResiliencePolicy = MethodsWithResiliencePolicy
                    .Concat(new[] { new ResiliencePolicyPredicatePair<TRequest, TResponse>(provider, predicate) })
                    .ToArray()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithResiliencePolicy(IEnumerable<Func<MethodInfo, IRequest, bool>> predicates, IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                MethodsWithResiliencePolicy = MethodsWithResiliencePolicy
                    .Concat(predicates.Select(predicate => new ResiliencePolicyPredicatePair<TRequest, TResponse>(provider, predicate)))
                    .ToArray()
            };
        }

        public BuilderContext<TRequest, TResponse> WithoutResiliencePolicy()
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                AllMethodsResiliencePolicyProvider = null,
                MethodsWithResiliencePolicy = Array.Empty<ResiliencePolicyPredicatePair<TRequest, TResponse>>()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithResultBuilders(IEnumerable<IResultBuilderProvider<IResponse>> resultBuilderProviders)
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
                ResultBuilderProviders = Array.Empty<IResultBuilderProvider<IResponse>>(),
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
            if (TransportProvider is null || TransportMessageBuilderProvider is null)
                throw _clientBuildExceptionFactory.HttpClientIsNotSet();
            if (SerializerProvider is null)
                throw _clientBuildExceptionFactory.SerializerIsNotSet();
        }
    }
}
