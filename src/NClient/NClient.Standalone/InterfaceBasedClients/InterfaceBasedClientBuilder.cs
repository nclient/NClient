using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Core.Helpers;
using NClient.Core.HttpClients;
using NClient.Core.Interceptors;
using NClient.Core.Interceptors.ClientInvocations;
using NClient.Core.Mappers;
using NClient.Core.RequestBuilders;
using NClient.Core.Resilience;

namespace NClient.InterfaceBasedClients
{
    public interface IInterfaceBasedClientBuilder<T> where T : class
    {
        IInterfaceBasedClientBuilder<T> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        IInterfaceBasedClientBuilder<T> WithLogging(ILogger<T> logger);
        T Build();
    }

    internal class InterfaceBasedClientBuilder<T> : IInterfaceBasedClientBuilder<T> where T : class
    {
        private readonly Uri _host;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IProxyGenerator _proxyGenerator;
        private IResiliencePolicyProvider? _resiliencePolicyProvider;
        private ILogger<T>? _logger;

        public InterfaceBasedClientBuilder(Uri host, IHttpClientProvider httpClientProvider, IProxyGenerator proxyGenerator)
        {
            _host = host;
            _httpClientProvider = httpClientProvider;
            _proxyGenerator = proxyGenerator;
        }

        public IInterfaceBasedClientBuilder<T> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            _resiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public IInterfaceBasedClientBuilder<T> WithLogging(ILogger<T> logger)
        {
            _logger = logger;
            return this;
        }

        public T Build()
        {
            var attributeMapper = new AttributeMapper();
            var clientInvocationProvider = new ClientInvocationProvider(_proxyGenerator);

            var requestBuilder = new RequestBuilder(
                _host,
                new RouteTemplateProvider(attributeMapper),
                new RouteProvider(),
                new HttpMethodProvider(attributeMapper),
                new ParameterProvider(attributeMapper),
                new ObjectToKeyValueConverter());

            var resilienceHttpClientProvider = new ResilienceHttpClientProvider(
                _httpClientProvider,
                _resiliencePolicyProvider ?? new StubResiliencePolicyProvider(),
                _logger);

            var interceptor = new ClientInterceptor<T>(
                resilienceHttpClientProvider,
                clientInvocationProvider,
                requestBuilder,
                controllerType: null,
                _logger);

            return (T)_proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToProxy: typeof(T),
                additionalInterfacesToProxy: new[] { typeof(IResilienceNClient<T>), typeof(IHttpNClient<T>) },
                interceptors: interceptor.ToInterceptor());
        }
    }
}
