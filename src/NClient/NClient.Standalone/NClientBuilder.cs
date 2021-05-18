using System;
using Castle.DynamicProxy;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.ControllerBasedClients;
using NClient.InterfaceBasedClients;

namespace NClient
{
    public interface INClientBuilder
    {
        IInterfaceBasedClientBuilder<T> Use<T>(string host, IHttpClientProvider httpClientProvider, ISerializerProvider serializerProvider) where T : class;

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(
            string host, IHttpClientProvider httpClientProvider, ISerializerProvider serializerProvider)
            where TInterface : class
            where TController : TInterface;
    }

    public class NClientBuilder : INClientBuilder
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();
        private static readonly InterfaceBasedClientValidator InterfaceBasedValidator = new();
        private static readonly ControllerBasedClientValidator ControllerBasedClientValidator = new();

        public IInterfaceBasedClientBuilder<T> Use<T>(string host, IHttpClientProvider httpClientProvider, ISerializerProvider serializerProvider)
            where T : class
        {
            InterfaceBasedValidator.Ensure<T>(ProxyGenerator);
            return new InterfaceBasedClientBuilder<T>(new Uri(host), httpClientProvider, serializerProvider, ProxyGenerator);
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(
            string host, IHttpClientProvider httpClientProvider, ISerializerProvider serializerProvider)
            where TInterface : class
            where TController : TInterface
        {
            ControllerBasedClientValidator.Ensure<TInterface, TController>(ProxyGenerator);
            return new ControllerBasedClientBuilder<TInterface, TController>(new Uri(host), httpClientProvider, serializerProvider, ProxyGenerator);
        }
    }
}
