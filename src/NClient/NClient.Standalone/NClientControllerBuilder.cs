using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Core.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.RequestBuilders;
using NClient.Core.Resilience;
using NClient.Standalone.Mappers;
using NClient.Standalone.Validators;

namespace NClient.Standalone
{
    [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use NClientBuilder.")]
    public interface INClientControllerBuilder
    {
        INClientControllerBuilder<TInterface, TController> Use<TInterface, TController>(
            string host, IHttpClientProvider httpClientProvider)
            where TInterface : class
            where TController : TInterface;
    }

    public interface INClientControllerBuilder<TInterface, TController>
        where TInterface : class
        where TController : TInterface
    {
        INClientControllerBuilder<TInterface, TController> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        INClientControllerBuilder<TInterface, TController> WithLogging(ILogger<TInterface> logger);
        TInterface Build();
    }

    [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use NClientBuilder.")]
    public class NClientControllerBuilder : INClientControllerBuilder
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();
        private static readonly ClientControllerValidator Validator = new();

        public INClientControllerBuilder<TInterface, TController> Use<TInterface, TController>(
            string host, IHttpClientProvider httpClientProvider)
            where TInterface : class
            where TController : TInterface
        {
            Validator.Ensure<TInterface, TController>(ProxyGenerator);
            return new NClientControllerBuilder<TInterface, TController>(new Uri(host), httpClientProvider, ProxyGenerator);
        }
    }

    internal class NClientControllerBuilder<TInterface, TController> : INClientControllerBuilder<TInterface, TController>
        where TInterface : class
        where TController : TInterface
    {
        private readonly Uri _host;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IProxyGenerator _proxyGenerator;
        private IResiliencePolicyProvider? _resiliencePolicyProvider;
        private ILogger<TInterface>? _logger;

        public NClientControllerBuilder(Uri host, IHttpClientProvider httpClientProvider, IProxyGenerator proxyGenerator)
        {
            _host = host;
            _httpClientProvider = httpClientProvider;
            _proxyGenerator = proxyGenerator;
        }

        public INClientControllerBuilder<TInterface, TController> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            _resiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public INClientControllerBuilder<TInterface, TController> WithLogging(ILogger<TInterface> logger)
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
