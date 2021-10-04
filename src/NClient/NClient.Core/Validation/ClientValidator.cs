using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience.Providers;
using NClient.Core.Handling;
using NClient.Core.HttpClients;
using NClient.Core.Interceptors;
using NClient.Core.Interceptors.Validation;
using NClient.Core.Serialization;
using NClient.Exceptions;

namespace NClient.Core.Validation
{
    internal interface IClientValidator
    {
        Task EnsureAsync<TClient>(IClientInterceptorFactory clientInterceptorFactory)
            where TClient : class;
    }

    internal class ClientValidator : IClientValidator
    {
        private static readonly Uri FakeHost = new("http://localhost:5000");

        private readonly IProxyGenerator _proxyGenerator;

        public ClientValidator(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
        }

        public async Task EnsureAsync<TClient>(IClientInterceptorFactory clientInterceptorFactory)
            where TClient : class
        {
            var interceptor = clientInterceptorFactory
                .Create<TClient, HttpRequest, HttpResponse>(
                    FakeHost,
                    new StubHttpClientProvider(),
                    new StubHttpMessageBuilderProvider(),
                    new StubSerializerProvider(),
                    new[] { new StubClientHandler<HttpRequest, HttpResponse>() },
                    new ResponseValidator<HttpRequest, HttpResponse>(new NoEnsuringSettings<HttpRequest, HttpResponse>()),
                    new MethodResiliencePolicyProviderAdapter<HttpRequest, HttpResponse>(
                        new NoResiliencePolicyProvider<HttpRequest, HttpResponse>()));
            var client = _proxyGenerator.CreateInterfaceProxyWithoutTarget<TClient>(interceptor.ToInterceptor());

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
