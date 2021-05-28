using System;
using Castle.DynamicProxy;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.ControllerBasedClients;
using NClient.InterfaceBasedClients;

namespace NClient
{
    public class NClientStandaloneBuilder : INClientBuilder
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ISerializerProvider _serializerProvider;
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();
        private static readonly InterfaceBasedClientValidator InterfaceBasedValidator = new();
        private static readonly ControllerBasedClientValidator ControllerBasedClientValidator = new();

        public NClientStandaloneBuilder(IHttpClientProvider httpClientProvider, ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            
            _httpClientProvider = httpClientProvider;
            _serializerProvider = serializerProvider;
        }

        public IInterfaceBasedClientBuilder<TInterface> Use<TInterface>(string host) 
            where TInterface : class
        {
            Ensure.IsNotNull(host, nameof(host));
            
            InterfaceBasedValidator.Ensure<TInterface>(ProxyGenerator);
            return new InterfaceBasedClientBuilder<TInterface>(new Uri(host), _httpClientProvider, _serializerProvider, ProxyGenerator);
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(string host)
            where TInterface : class
            where TController : TInterface
        {
            Ensure.IsNotNull(host, nameof(host));
            
            ControllerBasedClientValidator.Ensure<TInterface, TController>(ProxyGenerator);
            return new ControllerBasedClientBuilder<TInterface, TController>(new Uri(host), _httpClientProvider, _serializerProvider, ProxyGenerator);
        }
    }
}
