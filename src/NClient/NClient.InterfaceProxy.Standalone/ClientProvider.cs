using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Core.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.Mappers;
using NClient.Core.RequestBuilders;
using NClient.Core.Resilience;
using NClient.InterfaceProxy.Validators;

namespace NClient.InterfaceProxy
{
    public interface IClientProvider
    {
        IClientProvider<T> Use<T>(string host, IHttpClientProvider httpClientProvider) where T : class, INClient;
    }

    public interface IClientProvider<T> where T : class, INClient
    {
        IClientProvider<T> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        IClientProvider<T> WithLogging(ILogger<T> logger);
        T Build();
    }

    public class ClientProvider : IClientProvider
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();
        private static readonly ClientInterfaceValidator Validator = new();

        public IClientProvider<T> Use<T>(string host, IHttpClientProvider httpClientProvider) where T : class, INClient
        {
            Validator.Ensure<T>(ProxyGenerator);
            return new ClientProvider<T>(new Uri(host), httpClientProvider, ProxyGenerator);
        }
    }

    internal class ClientProvider<T> : IClientProvider<T> where T : class, INClient
    {
        private readonly Uri _host;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IProxyGenerator _proxyGenerator;
        private IResiliencePolicyProvider? _resiliencePolicyProvider;
        private ILogger<T>? _logger;

        public ClientProvider(Uri host, IHttpClientProvider httpClientProvider, IProxyGenerator proxyGenerator)
        {
            _host = host;
            _httpClientProvider = httpClientProvider;
            _proxyGenerator = proxyGenerator;
        }


        public IClientProvider<T> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            _resiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public IClientProvider<T> WithLogging(ILogger<T> logger)
        {
            _logger = logger;
            return this;
        }

        public T Build()
        {
            var attributeMapper = new AttributeMapper();

            var requestBuilder = new RequestBuilder(
                _host,
                new RouteTemplateProvider(attributeMapper),
                new RouteProvider(), 
                new HttpMethodProvider(attributeMapper), 
                new ParameterProvider(attributeMapper),
                new ObjectToKeyValueConverter());

            var interceptor = new ClientInterceptor<T>(
                _proxyGenerator, 
                _httpClientProvider, 
                requestBuilder, 
                _resiliencePolicyProvider ?? new StubResiliencePolicyProvider(), 
                controllerType: null, 
                _logger);

            return (T) _proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToProxy: typeof(T), 
                additionalInterfacesToProxy: new[] { typeof(IResilienceNClient<T>), typeof(IHttpNClient<T>) }, 
                interceptors: interceptor.ToInterceptor());
        }
    }
}
