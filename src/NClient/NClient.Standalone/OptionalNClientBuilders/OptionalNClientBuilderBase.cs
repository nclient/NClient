using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Core.Interceptors;

namespace NClient.OptionalNClientBuilders
{
    internal abstract class OptionalNClientBuilderBase<TInterface> : IOptionalNClientBuilder<TInterface>
        where TInterface : class
    {
        protected readonly Uri Host;
        protected readonly IClientInterceptorFactory ClientInterceptorFactory;
        protected readonly IProxyGenerator ProxyGenerator;
        protected IHttpClientProvider HttpClientProvider;
        protected ISerializerProvider SerializerProvider;
        protected IResiliencePolicyProvider? ResiliencePolicyProvider;
        protected ILogger<TInterface>? Logger;

        protected OptionalNClientBuilderBase(
            Uri host,
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IClientInterceptorFactory clientInterceptorFactory,
            IProxyGenerator proxyGenerator)
        {
            Host = host;
            HttpClientProvider = httpClientProvider;
            SerializerProvider = serializerProvider;
            ClientInterceptorFactory = clientInterceptorFactory;
            ProxyGenerator = proxyGenerator;
        }

        public IOptionalNClientBuilder<TInterface> WithCustomHttpClient(IHttpClientProvider httpClientProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));

            HttpClientProvider = httpClientProvider;
            return this;
        }

        public IOptionalNClientBuilder<TInterface> WithCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            SerializerProvider = serializerProvider;
            return this;
        }

        public IOptionalNClientBuilder<TInterface> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            Ensure.IsNotNull(resiliencePolicyProvider, nameof(resiliencePolicyProvider));

            ResiliencePolicyProvider = resiliencePolicyProvider;
            return this;
        }

        public IOptionalNClientBuilder<TInterface> WithLogging(ILogger<TInterface> logger)
        {
            Ensure.IsNotNull(logger, nameof(logger));

            Logger = logger;
            return this;
        }

        public abstract TInterface Build();

        protected TInterface CreateClient(IAsyncInterceptor asyncInterceptor)
        {
            return (TInterface)ProxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToProxy: typeof(TInterface),
                additionalInterfacesToProxy: new[] { typeof(IResilienceNClient<TInterface>), typeof(IHttpNClient<TInterface>) },
                interceptors: asyncInterceptor.ToInterceptor());
        }
    }
}