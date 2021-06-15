﻿using System;
using Castle.DynamicProxy;
using NClient.Abstractions;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.ClientGeneration;
using NClient.Core.Interceptors;
using NClient.OptionalNClientBuilders.Bases;

namespace NClient.OptionalNClientBuilders
{
    internal class OptionalInterfaceNClientBuilder<TInterface> :
        OptionalNClientBuilderBase<IOptionalNClientBuilder<TInterface>, TInterface>,
        IOptionalNClientBuilder<TInterface>
        where TInterface : class
    {
        private readonly Uri _host;
        private readonly IClientGenerator _clientGenerator;
        private readonly IClientInterceptorFactory _clientInterceptorFactory;

        public OptionalInterfaceNClientBuilder(
            Uri host,
            IClientGenerator clientGenerator,
            IClientInterceptorFactory clientInterceptorFactory,
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider)
            : base(httpClientProvider, serializerProvider)
        {
            _host = host;
            _clientGenerator = clientGenerator;
            _clientInterceptorFactory = clientInterceptorFactory;
        }

        public override TInterface Build()
        {
            var interceptor = _clientInterceptorFactory
                .Create(_host, HttpClientProvider, SerializerProvider, ResiliencePolicyProvider, Logger);
            return _clientGenerator.CreateClient<TInterface>(interceptor);
        }
    }
}
