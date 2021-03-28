using System;
using Castle.DynamicProxy;
using NClient.Abstractions.HttpClients;
using NClient.Standalone.ControllerBasedClients;
using NClient.Standalone.InterfaceBasedClients;

namespace NClient.Standalone
{
    public interface INClientBuilder
    {
        IInterfaceBasedClientBuilder<T> Use<T>(string host, IHttpClientProvider httpClientProvider) where T : class;

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(
            string host, IHttpClientProvider httpClientProvider)
            where TInterface : class
            where TController : TInterface;
    }

    public class NClientBuilder : INClientBuilder
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();
        private static readonly InterfaceBasedClientValidator InterfaceBasedValidator = new();
        private static readonly ControllerBasedClientValidator ControllerBasedClientValidator = new();

        public IInterfaceBasedClientBuilder<T> Use<T>(string host, IHttpClientProvider httpClientProvider) where T : class
        {
            InterfaceBasedValidator.Ensure<T>(ProxyGenerator);
            return new InterfaceBasedClientBuilder<T>(new Uri(host), httpClientProvider, ProxyGenerator);
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(
            string host, IHttpClientProvider httpClientProvider)
            where TInterface : class
            where TController : TInterface
        {
            ControllerBasedClientValidator.Ensure<TInterface, TController>(ProxyGenerator);
            return new ControllerBasedClientBuilder<TInterface, TController>(new Uri(host), httpClientProvider, ProxyGenerator);
        }
    }
}
