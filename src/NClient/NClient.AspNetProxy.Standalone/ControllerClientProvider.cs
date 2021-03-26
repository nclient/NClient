using System;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.AspNetProxy.Attributes;
using NClient.AspNetProxy.Validators;
using NClient.Core.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.RequestBuilders;
using NClient.Core.Resilience;

namespace NClient.AspNetProxy
{
    public interface IControllerClientProvider
    {
        IControllerClientProviderHttp<TInterface, TController> Use<TInterface, TController>(string host)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface;
        IControllerClientProviderHttp<TInterface, TController> Use<TInterface, TController>(Uri host)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface;
    }

    public interface IControllerClientProviderHttp<TInterface, TController>
        where TInterface : class, INClient
        where TController : ControllerBase, TInterface
    {
        IControllerClientProviderResilience<TInterface, TController> SetHttpClientProvider(IHttpClientProvider httpClientProvider);
    }

    public interface IControllerClientProviderResilience<TInterface, TController>
        where TInterface : class, INClient
        where TController : ControllerBase, TInterface
    {
        IControllerClientProviderLogger<TInterface, TController> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        IControllerClientProviderLogger<TInterface, TController> WithoutResiliencePolicy();
    }

    public interface IControllerClientProviderLogger<TInterface, TController>
        where TInterface : class, INClient
        where TController : ControllerBase, TInterface
    {
        IControllerClientProviderLogger<TInterface, TController> WithLogger(ILogger<TInterface> logger);
        TInterface Build();
    }

    public class ControllerClientProvider : IControllerClientProvider
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();
        private static readonly ClientControllerValidator Validator = new();

        public IControllerClientProviderHttp<TInterface, TController> Use<TInterface, TController>(string host)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return Use<TInterface, TController>(new Uri(host));
        }

        public IControllerClientProviderHttp<TInterface, TController> Use<TInterface, TController>(Uri host)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            Validator.Ensure<TInterface, TController>(ProxyGenerator);
            return new ControllerClientProvider<TInterface, TController>(host, ProxyGenerator);
        }
    }

    internal class ControllerClientProvider<TInterface, TController> : 
        IControllerClientProviderHttp<TInterface, TController>, 
        IControllerClientProviderResilience<TInterface, TController>, 
        IControllerClientProviderLogger<TInterface, TController>
        where TInterface : class, INClient
        where TController : ControllerBase, TInterface
    {
        private readonly Uri _host;
        private readonly IProxyGenerator _proxyGenerator;
        private IHttpClientProvider _httpClientProvider = null!;
        private IResiliencePolicyProvider _resiliencePolicyProvider = null!;
        private ILogger<TInterface>? _logger;

        public ControllerClientProvider(Uri host, IProxyGenerator proxyGenerator)
        {
            _host = host;
            _proxyGenerator = proxyGenerator;
        }

        public IControllerClientProviderResilience<TInterface, TController> SetHttpClientProvider(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
            return this;
        }


        public IControllerClientProviderLogger<TInterface, TController> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            _resiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public IControllerClientProviderLogger<TInterface, TController> WithoutResiliencePolicy()
        {
            _resiliencePolicyProvider = new StubResiliencePolicyProvider();
            return this;
        }

        public IControllerClientProviderLogger<TInterface, TController> WithLogger(ILogger<TInterface> logger)
        {
            _logger = logger;
            return this;
        }

        public TInterface Build()
        {
            var attributeMapper = new AspNetAttributeMapper();

            var requestBuilder = new RequestBuilder(
                _host,
                new RouteTemplateProvider(attributeMapper),
                new RouteProvider(),
                new HttpMethodProvider(attributeMapper),
                new ParameterProvider(attributeMapper),
                new ObjectToKeyValueConverter());

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
