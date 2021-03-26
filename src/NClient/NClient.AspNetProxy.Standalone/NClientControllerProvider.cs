using System;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.AspNetProxy.Mappers;
using NClient.AspNetProxy.Validators;
using NClient.Core.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.RequestBuilders;
using NClient.Core.Resilience;

namespace NClient.AspNetProxy
{
    [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use NClientProvider.")]
    public interface INClientControllerProvider
    {
        INClientControllerProvider<TInterface, TController> Use<TInterface, TController>(
            string host, IHttpClientProvider httpClientProvider)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface;
    }

    public interface INClientControllerProvider<TInterface, TController>
        where TInterface : class, INClient
        where TController : ControllerBase, TInterface
    {
        INClientControllerProvider<TInterface, TController> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        INClientControllerProvider<TInterface, TController> WithLogging(ILogger<TInterface> logger);
        TInterface Build();
    }

    [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use NClientProvider.")]
    public class NClientControllerProvider : INClientControllerProvider
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();
        private static readonly ClientControllerValidator Validator = new();

        public INClientControllerProvider<TInterface, TController> Use<TInterface, TController>(
            string host, IHttpClientProvider httpClientProvider)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            Validator.Ensure<TInterface, TController>(ProxyGenerator);
            return new ControllerClientProvider<TInterface, TController>(new Uri(host), httpClientProvider, ProxyGenerator);
        }
    }

    internal class ControllerClientProvider<TInterface, TController> : INClientControllerProvider<TInterface, TController>
        where TInterface : class, INClient
        where TController : ControllerBase, TInterface
    {
        private readonly Uri _host;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IProxyGenerator _proxyGenerator;
        private IResiliencePolicyProvider? _resiliencePolicyProvider;
        private ILogger<TInterface>? _logger;

        public ControllerClientProvider(Uri host, IHttpClientProvider httpClientProvider, IProxyGenerator proxyGenerator)
        {
            _host = host;
            _httpClientProvider = httpClientProvider;
            _proxyGenerator = proxyGenerator;
        }

        public INClientControllerProvider<TInterface, TController> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            _resiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public INClientControllerProvider<TInterface, TController> WithLogging(ILogger<TInterface> logger)
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
                _resiliencePolicyProvider ?? new StubResiliencePolicyProvider(),
                controllerType: typeof(TController),
                _logger);
            
            return (TInterface) _proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToProxy: typeof(TInterface), 
                additionalInterfacesToProxy: new[] { typeof(IResilienceNClient<TInterface>), typeof(IHttpNClient<TInterface>) },
                interceptors: interceptor.ToInterceptor());
        }
    }
}
