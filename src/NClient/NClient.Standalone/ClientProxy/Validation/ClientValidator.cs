using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using NClient.Exceptions;
using NClient.Providers.Mapping;
using NClient.Providers.Transport;
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
        Task EnsureAsync<TClient>()
            where TClient : class;
    }

    internal class ClientValidator<TRequest, TResponse>: IClientValidator
    {
        private static readonly Uri FakeHost = new("http://localhost:5000");

        private readonly IClientProxyGenerator _clientProxyGenerator;
        private readonly BuilderContext<IRequest, IResponse> _builderContext;
        private readonly IClientInterceptorFactory _clientInterceptorFactory;
        private IAsyncInterceptor _interceptor;

        public ClientValidator(IProxyGenerator proxyGenerator,
            IClientInterceptorFactory clientInterceptorFactory,
            BuilderContext<TRequest, TResponse> builderContext)
        {
            _clientInterceptorFactory = clientInterceptorFactory;
            _clientProxyGenerator = new ClientProxyGenerator(proxyGenerator, new ClientValidationExceptionFactory());
            _builderContext = new BuilderContext<IRequest, IResponse>()
                           .WithHost(FakeHost)
                           .WithSerializer(new StubSerializerProvider())
                           .WithRequestBuilderProvider(builderContext.RequestBuilderProvider)
                           .WithTransport(
                               new StubTransportProvider<IRequest, IResponse>(),
                               new StubTransportRequestBuilderProvider<TRequest, TResponse>(),
                               new StubResponseBuilderProvider<IRequest, IResponse>())
                           .WithAuthorization(new[] { new StubAuthorizationProvider() })
                           .WithHandlers(new[] { new StubClientHandlerProvider<IRequest, IResponse>() })
                           .WithResiliencePolicy(new MethodResiliencePolicyProviderAdapter<IRequest, IResponse>(
                               new StubResiliencePolicyProvider<IRequest, IResponse>()))
                           .WithResponseMapperProviders(Array.Empty<IResponseMapperProvider<IRequest, IResponse>>())
                           .WithTransportResponseMapperProviders(Array.Empty<IResponseMapperProvider<IRequest, IResponse>>())
                           .WithResponseValidation(new[] { new StubResponseValidatorProvider<IRequest, IResponse>() });
        }

        public async Task EnsureAsync<TClient>()
            where TClient : class
        {
            BuilderContext<IRequest, IResponse> validationContext = _builderContext.WithHost(FakeHost)
                .WithTransport(
                    new StubTransportProvider<TRequest, TResponse>(),
                    new StubTransportRequestBuilderProvider<TRequest, TResponse>(),
                    new StubResponseBuilderProvider<TRequest, TResponse>())
                .WithoutAuthorization()
                .WithoutHandlers()
                .WithoutResiliencePolicy()
                .WithoutAllResponseMapperProviders()
                .WithoutResponseValidation()
                .WithoutLogging();

            _interceptor = _clientInterceptorFactory.Create<TClient, IRequest, IResponse>(validationContext);
            var client = _clientProxyGenerator.CreateClient<TClient>(_interceptor);

            await EnsureValidityAsync(client).ConfigureAwait(false);
        }
        



        private async Task EnsureValidityAsync<T>(T client) where T : class
        {
            var methods = NClient.Core.Helpers.TypeExtensions.GetUnhiddenInterfaceMethods(typeof(T), true);

            foreach (var methodInfo in methods)
            {
                var parameters = methodInfo.GetParameters().Select(GetDefaultParameter).ToArray();
               
                try
                {
                    var result = methodInfo.Invoke(client, parameters);
                    if (_clientInterceptorFactory.PipelineCanceler.IsCancellationRequested)
                    {
                        _clientInterceptorFactory.PipelineCanceler.Renew();
                        continue;
                    }
                    
                    if (result is Task task)
                        await task.ConfigureAwait(false);
                }
                catch (ClientValidationException)
                {
                    throw;
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
            while(innerEx != null)
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
