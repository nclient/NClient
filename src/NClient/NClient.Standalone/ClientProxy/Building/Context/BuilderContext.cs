using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.Invocation;
using NClient.Providers.Api;
using NClient.Providers.Handling;
using NClient.Providers.Mapping;
using NClient.Providers.Resilience;
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
        
        public Uri BaseUri { get; private set; } = null!;

        public ITransportProvider<TRequest, TResponse> TransportProvider { get; private set; } = null!;
        public ITransportRequestBuilderProvider<TRequest, TResponse> TransportRequestBuilderProvider { get; private set; } = null!;
        public IResponseBuilderProvider<TRequest, TResponse> ResponseBuilderProvider { get; private set; } = null!;
        
        public IRequestBuilderProvider RequestBuilderProvider { get; private set; } = null!;
        
        public ISerializerProvider SerializerProvider { get; private set; } = null!;

        public IReadOnlyCollection<IResponseValidatorProvider<TRequest, TResponse>> ResponseValidatorProviders { get; private set; }

        public IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> ClientHandlerProviders { get; private set; }

        public IMethodResiliencePolicyProvider<TRequest, TResponse>? AllMethodsResiliencePolicyProvider { get; private set; }
        public IReadOnlyCollection<ResiliencePolicyPredicate<TRequest, TResponse>> MethodsWithResiliencePolicy { get; private set; }
        
        public IReadOnlyCollection<IResponseMapperProvider<IRequest, IResponse>> ResultBuilderProviders { get; private set; }
        public IReadOnlyCollection<IResponseMapperProvider<TRequest, TResponse>> TypedResultBuilderProviders { get; private set; }

        public TimeSpan? Timeout { get; private set; }
        
        public IReadOnlyCollection<ILogger> Loggers { get; private set; }
        public ILoggerFactory? LoggerFactory { get; private set; }

        public BuilderContext()
        {
            ResponseValidatorProviders = Array.Empty<IResponseValidatorProvider<TRequest, TResponse>>();
            ClientHandlerProviders = Array.Empty<IClientHandlerProvider<TRequest, TResponse>>();
            MethodsWithResiliencePolicy = Array.Empty<ResiliencePolicyPredicate<TRequest, TResponse>>();
            ResultBuilderProviders = Array.Empty<IResponseMapperProvider<IRequest, IResponse>>();
            TypedResultBuilderProviders = Array.Empty<IResponseMapperProvider<TRequest, TResponse>>();
            Loggers = Array.Empty<ILogger>();
            _clientBuildExceptionFactory = new ClientBuildExceptionFactory();
        }

        public BuilderContext(BuilderContext<TRequest, TResponse> builderContext)
        {
            _clientBuildExceptionFactory = builderContext._clientBuildExceptionFactory;
            
            BaseUri = builderContext.BaseUri;

            TransportProvider = builderContext.TransportProvider;
            TransportRequestBuilderProvider = builderContext.TransportRequestBuilderProvider;
            ResponseBuilderProvider = builderContext.ResponseBuilderProvider;
            
            RequestBuilderProvider = builderContext.RequestBuilderProvider;

            SerializerProvider = builderContext.SerializerProvider;

            ResponseValidatorProviders = builderContext.ResponseValidatorProviders.ToArray();

            ClientHandlerProviders = builderContext.ClientHandlerProviders.ToArray();

            AllMethodsResiliencePolicyProvider = builderContext.AllMethodsResiliencePolicyProvider;
            MethodsWithResiliencePolicy = builderContext.MethodsWithResiliencePolicy.ToArray();

            ResultBuilderProviders = builderContext.ResultBuilderProviders.ToArray();
            TypedResultBuilderProviders = builderContext.TypedResultBuilderProviders.ToArray();

            Timeout = builderContext.Timeout;
            
            Loggers = builderContext.Loggers.ToArray();
            LoggerFactory = builderContext.LoggerFactory;
        }

        public BuilderContext<TRequest, TResponse> WithBaseUri(Uri baseUri)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                BaseUri = baseUri
            };
        }

        public BuilderContext<TRequest, TResponse> WithTransport(
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportRequestBuilderProvider<TRequest, TResponse> transportRequestBuilderProvider,
            IResponseBuilderProvider<TRequest, TResponse> responseBuilderProvider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                TransportProvider = transportProvider,
                TransportRequestBuilderProvider = transportRequestBuilderProvider,
                ResponseBuilderProvider = responseBuilderProvider
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithRequestBuilderProvider(IRequestBuilderProvider requestBuilderProvider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                RequestBuilderProvider = requestBuilderProvider
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

        public BuilderContext<TRequest, TResponse> WithResiliencePolicy(Func<IMethod, IRequest, bool> predicate, IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                MethodsWithResiliencePolicy = MethodsWithResiliencePolicy
                    .Concat(new[] { new ResiliencePolicyPredicate<TRequest, TResponse>(provider, predicate) })
                    .ToArray()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithResiliencePolicy(IEnumerable<Func<IMethod, IRequest, bool>> predicates, IResiliencePolicyProvider<TRequest, TResponse> provider)
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
        
        public BuilderContext<TRequest, TResponse> WithResultBuilders(IEnumerable<IResponseMapperProvider<IRequest, IResponse>> resultBuilderProviders)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                ResultBuilderProviders = ResultBuilderProviders.Concat(resultBuilderProviders).ToArray()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithResultBuilders(IEnumerable<IResponseMapperProvider<TRequest, TResponse>> resultBuilderProviders)
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
                ResultBuilderProviders = Array.Empty<IResponseMapperProvider<IRequest, IResponse>>(),
                TypedResultBuilderProviders = Array.Empty<IResponseMapperProvider<TRequest, TResponse>>()
            };
        }
        
        public BuilderContext<TRequest, TResponse> WithTimeout(TimeSpan timeout)
        {
            return new BuilderContext<TRequest, TResponse>(this)
            {
                Timeout = timeout
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
            if (BaseUri is null) 
                throw _clientBuildExceptionFactory.HostIsNotSet();
            if (RequestBuilderProvider is null)
                throw _clientBuildExceptionFactory.ApiIsNotSet();
            if (TransportProvider is null || TransportRequestBuilderProvider is null || ResponseBuilderProvider is null)
                throw _clientBuildExceptionFactory.TransportIsNotSet();
            if (SerializerProvider is null)
                throw _clientBuildExceptionFactory.SerializerIsNotSet();
        }
    }
}
