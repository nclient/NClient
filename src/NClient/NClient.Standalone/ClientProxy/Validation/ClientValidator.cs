using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using NClient.Exceptions;
using NClient.Providers.Mapping;
using NClient.Providers.Transport;
using NClient.Providers.Transport.Common;
using NClient.Standalone.Client.Resilience;
using NClient.Standalone.ClientProxy.Building.Context;
using NClient.Standalone.ClientProxy.Generation;
using NClient.Standalone.ClientProxy.Generation.Interceptors;
using NClient.Standalone.ClientProxy.Validation.Authorization;
using NClient.Standalone.ClientProxy.Validation.Handling;
using NClient.Standalone.ClientProxy.Validation.Resilience;
using NClient.Standalone.ClientProxy.Validation.Serialization;
using NClient.Standalone.ClientProxy.Validation.Transport;
using NClient.Standalone.ClientProxy.Validation.Validation;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Validation
{
    internal interface IClientValidator
    {
        void Ensure<TClient>()
            where TClient : class;
    }

    internal class ClientValidator<TRequest, TResponse> : IClientValidator
    {
        private static readonly Uri FakeHost = new("http://localhost:5000");

        private readonly IClientProxyGenerator _clientProxyGenerator;
        private readonly BuilderContext<IRequest, IResponse> _builderContext;
        private readonly IClientInterceptorFactory _clientInterceptorFactory;
        private IAsyncInterceptor? _interceptor;
        private readonly IPipelineCanceller _pipelineCanceler;

        public ClientValidator(IProxyGenerator proxyGenerator,
            IClientInterceptorFactory clientInterceptorFactory,
            BuilderContext<TRequest, TResponse> builderContext)
        {
            _pipelineCanceler = new PipelineCanceller();
            _clientInterceptorFactory = clientInterceptorFactory;
            _clientProxyGenerator = new ClientProxyGenerator(proxyGenerator, new ClientValidationExceptionFactory());
            _builderContext = new BuilderContext<IRequest, IResponse>()
                .WithHost(FakeHost)
                .WithSerializer(new StubSerializerProvider())
                .WithRequestBuilderProvider(builderContext.RequestBuilderProvider)
                .WithTransport(
                    new StubTransportProvider(),
                    new StubTransportRequestBuilderProvider(),
                    new StubResponseBuilderProvider())
                .WithAuthorization(new[] { new StubAuthorizationProvider() })
                .WithHandlers(new[] { new StubClientHandlerProvider<IRequest, IResponse>() })
                .WithResiliencePolicy(new MethodResiliencePolicyProviderAdapter<IRequest, IResponse>(
                    new StubResiliencePolicyProvider<IRequest, IResponse>()))
                .WithResponseMapperProviders(Array.Empty<IResponseMapperProvider<IRequest, IResponse>>())
                .WithTransportResponseMapperProviders(Array.Empty<IResponseMapperProvider<IRequest, IResponse>>())
                .WithResponseValidation(new[] { new StubResponseValidatorProvider<IRequest, IResponse>() });
        }

        public void Ensure<TClient>()
            where TClient : class
        {
            var validationContext = _builderContext.WithHost(FakeHost)
                .WithTransport(
                    new StubTransportProvider(),
                    new StubTransportRequestBuilderProvider(),
                    new StubResponseBuilderProvider())
                .WithoutAuthorization()
                .WithoutHandlers()
                .WithoutResiliencePolicy()
                .WithoutAllResponseMapperProviders()
                .WithoutResponseValidation()
                .WithoutLogging();

            _interceptor = _clientInterceptorFactory.Create<TClient, IRequest, IResponse>(validationContext, _pipelineCanceler);
            var client = _clientProxyGenerator.CreateClient<TClient>(_interceptor);

            EnsureValidity(client);
        }
        
        private void EnsureValidity<T>(T client) where T : class
        {
            var methods = Core.Helpers.TypeExtensions.GetUnhiddenInterfaceMethods(typeof(T), true);

            foreach (var methodInfo in methods)
            {
                var parameters = methodInfo.GetParameters().Select(GetDefaultParameter).ToArray();
               
                try
                {
                    methodInfo.Invoke(client, parameters);
                    if (_clientInterceptorFactory.PipelineCanceler.IsCancellationRequested)
                        _clientInterceptorFactory.PipelineCanceler.Renew();
                }
                catch (TargetInvocationException tex)
                {
                    TraverseInnerExceptionTree(tex);
                }
            }
        }
        
        private void TraverseInnerExceptionTree(Exception ex)
        {
            var innerEx = ex.InnerException;
            while (innerEx != null)
            {
                if (innerEx.GetType() == typeof(ClientValidationException))
                    throw innerEx;

                innerEx = innerEx.InnerException;
            }
        }

        private object? GetDefaultParameter(ParameterInfo parameter)
        {
            return parameter.ParameterType.IsValueType
                ? Activator.CreateInstance(parameter.ParameterType)
                : null;
        }
    }
}
