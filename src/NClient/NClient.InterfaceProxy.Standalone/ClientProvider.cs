using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Core;
using NClient.Core.Attributes;
using NClient.Core.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.RequestBuilders;
using NClient.InterfaceProxy.Validators;
using NClient.Providers.HttpClient.Abstractions;
using NClient.Providers.Resilience;
using NClient.Providers.Resilience.Abstractions;

namespace NClient.InterfaceProxy
{
    public interface IClientProvider
    {
        IClientProviderHttp<T> Use<T>(Uri host) where T : class, INClient;
    }

    public interface IClientProviderHttp<T> where T : class, INClient
    {
        IClientProviderResilience<T> SetHttpClientProvider(IHttpClientProvider httpClientProvider);
    }

    public interface IClientProviderResilience<T> where T : class, INClient
    {
        IClientProviderLogger<T> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        IClientProviderLogger<T> WithoutResiliencePolicy();
    }

    public interface IClientProviderLogger<T> where T : class, INClient
    {
        IClientProviderLogger<T> WithLogger(ILogger<T> logger);
        T Build();
    }

    public class ClientProvider : IClientProvider
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();
        private static readonly ClientInterfaceValidator Validator = new();

        public IClientProviderHttp<T> Use<T>(Uri host) where T : class, INClient
        {
            Validator.Ensure<T>(ProxyGenerator);
            return new ClientProvider<T>(host, ProxyGenerator);
        }
    }

    public class ClientProvider<T> : IClientProviderHttp<T>, IClientProviderResilience<T>, IClientProviderLogger<T> where T : class, INClient
    {
        private readonly Uri _host;
        private readonly IProxyGenerator _proxyGenerator;
        private IHttpClientProvider _httpClientProvider = null!;
        private IResiliencePolicyProvider _resiliencePolicyProvider = null!;
        private ILogger<T>? _logger;

        public ClientProvider(Uri host, IProxyGenerator proxyGenerator)
        {
            _host = host;
            _proxyGenerator = proxyGenerator;
        }

        public IClientProviderResilience<T> SetHttpClientProvider(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
            return this;
        }


        public IClientProviderLogger<T> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            _resiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public IClientProviderLogger<T> WithoutResiliencePolicy()
        {
            _resiliencePolicyProvider = new StubResiliencePolicyProvider();
            return this;
        }

        public IClientProviderLogger<T> WithLogger(ILogger<T> logger)
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
                _resiliencePolicyProvider, 
                controllerType: null, 
                _logger);

            return (T) _proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToProxy: typeof(T), 
                additionalInterfacesToProxy: new[] { typeof(IResilienceNClient<T>), typeof(IHttpNClient<T>) }, 
                interceptors: interceptor.ToInterceptor());
        }
    }
}
