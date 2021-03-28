﻿using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Core.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.RequestBuilders;
using NClient.Core.Resilience;
using NClient.Mappers;

namespace NClient.ControllerBasedClients
{
    public interface IControllerBasedClientBuilder<TInterface, TController>
        where TInterface : class
        where TController : TInterface
    {
        IControllerBasedClientBuilder<TInterface, TController> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        IControllerBasedClientBuilder<TInterface, TController> WithLogging(ILogger<TInterface> logger);
        TInterface Build();
    }

    internal class ControllerBasedClientBuilder<TInterface, TController> : IControllerBasedClientBuilder<TInterface, TController>
        where TInterface : class
        where TController : TInterface
    {
        private readonly Uri _host;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IProxyGenerator _proxyGenerator;
        private IResiliencePolicyProvider? _resiliencePolicyProvider;
        private ILogger<TInterface>? _logger;

        public ControllerBasedClientBuilder(Uri host, IHttpClientProvider httpClientProvider, IProxyGenerator proxyGenerator)
        {
            _host = host;
            _httpClientProvider = httpClientProvider;
            _proxyGenerator = proxyGenerator;
        }

        public IControllerBasedClientBuilder<TInterface, TController> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            _resiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public IControllerBasedClientBuilder<TInterface, TController> WithLogging(ILogger<TInterface> logger)
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
