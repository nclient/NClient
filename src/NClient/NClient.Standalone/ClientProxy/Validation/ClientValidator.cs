using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using NClient.Exceptions;
using NClient.Providers.Mapping;
using NClient.Providers.Transport;
using NClient.Standalone.Client.Resilience;
using NClient.Standalone.ClientProxy.Generation;
using NClient.Standalone.ClientProxy.Generation.Interceptors;
using NClient.Standalone.ClientProxy.Validation.Api;
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
        Task EnsureAsync<TClient>(IClientInterceptorFactory clientInterceptorFactory)
            where TClient : class;
    }

    internal class ClientValidator : IClientValidator
    {
        private static readonly Uri FakeHost = new("http://localhost:5000");

        private readonly IClientProxyGenerator _clientProxyGenerator;

        public ClientValidator(IProxyGenerator proxyGenerator)
        {
            _clientProxyGenerator = new ClientProxyGenerator(proxyGenerator, new ClientValidationExceptionFactory());
        }

        public async Task EnsureAsync<TClient>(IClientInterceptorFactory clientInterceptorFactory)
            where TClient : class
        {
            var interceptor = clientInterceptorFactory
                .Create<TClient, IRequest, IResponse>(
                    FakeHost,
                    new StubSerializerProvider(),
                    new StubRequestBuilderProvider(),
                    new StubTransportProvider(),
                    new StubTransportRequestBuilderProvider(),
                    new StubResponseBuilderProvider(),
                    new[] { new StubAuthorizationProvider() },
                    new[] { new StubClientHandlerProvider<IRequest, IResponse>() },
                    new MethodResiliencePolicyProviderAdapter<IRequest, IResponse>(
                        new StubResiliencePolicyProvider<IRequest, IResponse>()),
                    Array.Empty<IResponseMapperProvider<IRequest, IResponse>>(),
                    Array.Empty<IResponseMapperProvider<IRequest, IResponse>>(),
                    new[] { new StubResponseValidatorProvider<IRequest, IResponse>() });
            var client = _clientProxyGenerator.CreateClient<TClient>(interceptor);

            await EnsureValidityAsync(client).ConfigureAwait(false);
        }
        
        private static async Task EnsureValidityAsync<T>(T client) where T : class
        {
            foreach (var methodInfo in typeof(T).GetMethods())
            {
                var parameters = methodInfo.GetParameters().Select(GetDefaultParameter).ToArray();

                try
                {
                    var result = methodInfo.Invoke(client, parameters);
                    if (result is Task task)
                        await task.ConfigureAwait(false);
                }
                catch (ClientValidationException)
                {
                    throw;
                }
                catch
                {
                    // ignored
                }
            }
        }

        private static object? GetDefaultParameter(ParameterInfo parameter)
        {
            return parameter.ParameterType.IsValueType
                ? Activator.CreateInstance(parameter.ParameterType)
                : null;
        }
    }
}
