using System;
using System.Net.Http;
using NClient.ControllerBasedClients;
using NClient.InterfaceBasedClients;
using NClient.Providers.HttpClient.System;

namespace NClient
{
    public static class NClientProvider
    {
        public static IInterfaceBasedClientBuilder<T> Use<T>(string host) where T : class
        {
            return new NClientBuilder().Use<T>(host, new SystemHttpClientProvider());
        }

        public static IInterfaceBasedClientBuilder<T> Use<T>(string host, HttpMessageHandler httpMessageHandler) where T : class
        {
            return new NClientBuilder().Use<T>(host, new SystemHttpClientProvider(httpMessageHandler));
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(
            string host)
            where TInterface : class
            where TController : TInterface
        {
            return new NClientBuilder().Use<TInterface, TController>(host, new SystemHttpClientProvider());
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(
            string host, HttpMessageHandler httpMessageHandler)
            where TInterface : class
            where TController : TInterface
        {
            return new NClientBuilder().Use<TInterface, TController>(host, new SystemHttpClientProvider(httpMessageHandler));
        }
    }
}
