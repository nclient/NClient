using System;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NClient.AspNetProxy.Attributes;
using NClient.AspNetProxy.Validators;
using NClient.Core;
using NClient.Core.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.RequestBuilders;
using NClient.Providers.HttpClient.Abstractions;
using NClient.Providers.Resilience;
using NClient.Providers.Resilience.Abstractions;

namespace NClient.AspNetProxy
{
    public interface IClientProvider
    {
        IClientProviderHttp<TInterface, TController> Use<TInterface, TController>(Uri host)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface;
    }

    public interface IClientProviderHttp<TInterface, TController>
        where TInterface : class, INClient
        where TController : ControllerBase, TInterface
    {
        IClientProviderResilience<TInterface, TController> SetHttpClientProvider(IHttpClientProvider httpClientProvider);
    }

    public interface IClientProviderResilience<TInterface, TController>
        where TInterface : class, INClient
        where TController : ControllerBase, TInterface
    {
        IClientProviderLogger<TInterface, TController> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        IClientProviderLogger<TInterface, TController> WithoutResiliencePolicy();
    }

    public interface IClientProviderLogger<TInterface, TController>
        where TInterface : class, INClient
        where TController : ControllerBase, TInterface
    {
        IClientProviderLogger<TInterface, TController> WithLogger(ILogger<TInterface> logger);
        TInterface Build();
    }

    public class AspNetClientProvider : IClientProvider
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();
        private static readonly ClientControllerValidator Validator = new();

        public IClientProviderHttp<TInterface, TController> Use<TInterface, TController>(Uri host)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            Validator.Ensure<TInterface, TController>(ProxyGenerator);
            return new AspNetClientProvider<TInterface, TController>(host, ProxyGenerator);
        }
    }

    public class AspNetClientProvider<TInterface, TController> : 
        IClientProviderHttp<TInterface, TController>, 
        IClientProviderResilience<TInterface, TController>, 
        IClientProviderLogger<TInterface, TController>
        where TInterface : class, INClient
        where TController : ControllerBase, TInterface
    {
        private readonly Uri _host;
        private readonly IProxyGenerator _proxyGenerator;
        private IHttpClientProvider _httpClientProvider = null!;
        private IResiliencePolicyProvider _resiliencePolicyProvider = null!;
        private ILogger<TInterface>? _logger;

        public AspNetClientProvider(Uri host, IProxyGenerator proxyGenerator)
        {
            _host = host;
            _proxyGenerator = proxyGenerator;
        }

        public IClientProviderResilience<TInterface, TController> SetHttpClientProvider(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
            return this;
        }


        public IClientProviderLogger<TInterface, TController> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            _resiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public IClientProviderLogger<TInterface, TController> WithoutResiliencePolicy()
        {
            _resiliencePolicyProvider = new StubResiliencePolicyProvider();
            return this;
        }

        public IClientProviderLogger<TInterface, TController> WithLogger(ILogger<TInterface> logger)
        {
            _logger = logger;
            return this;
        }


        public TInterface Build()
        {
            var attributeHelper = new AspNetCoreAttributeHelper();

            var requestBuilder = new RequestBuilder(
                _host,
                new RouteBuilder(attributeHelper),
                new HttpMethodProvider(attributeHelper),
                new ParameterProvider(attributeHelper),
                new ObjectToKeyValueConverter(),
                attributeHelper);

            var interceptor = new ClientInterceptor<TInterface>(
                _proxyGenerator, 
                _httpClientProvider, 
                requestBuilder, 
                _resiliencePolicyProvider,
                controllerType: typeof(TController),
                _logger);
            
            return (TInterface) _proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToProxy: typeof(TInterface), 
                additionalInterfacesToProxy: new[] { typeof(IResilienceNClient<TInterface>), typeof(IHttpNClient<TInterface>) },
                interceptors: interceptor.ToInterceptor());
        }
    }
}
