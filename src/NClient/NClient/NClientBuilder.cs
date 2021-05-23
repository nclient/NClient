﻿using System;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Serialization.System;

namespace NClient
{
    public class NClientBuilder : INClientBuilder
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ISerializerProvider _serializerProvider;

        public NClientBuilder() : this(new SystemHttpClientProvider(), new SystemSerializerProvider())
        {
        }

        private NClientBuilder(IHttpClientProvider httpClientProvider, ISerializerProvider serializerProvider)
        {
            _httpClientProvider = httpClientProvider;
            _serializerProvider = serializerProvider;
        }

        public IInterfaceBasedClientBuilder<TInterface> Use<TInterface>(string host) where TInterface : class
        {
            return new NClientStandaloneBuilder(_httpClientProvider, _serializerProvider).Use<TInterface>(host);
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(string host)
            where TInterface : class
            where TController : TInterface
        {
            return new NClientStandaloneBuilder(_httpClientProvider, _serializerProvider).Use<TInterface, TController>(host);
        }
    }
}