using System;
using NClient.Abstractions.HttpClients;
using NClient.Standalone.ControllerBasedClients;
using NClient.Standalone.InterfaceBasedClients;

namespace NClient.Standalone
{
    public static class NClientStandaloneProvider
    {
        public static IInterfaceBasedClientBuilder<T> Use<T>(string host, IHttpClientProvider httpClientProvider) where T : class
        {
            return new NClientBuilder().Use<T>(host, httpClientProvider);
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(
            string host, IHttpClientProvider httpClientProvider)
            where TInterface : class
            where TController : TInterface
        {
            return new NClientBuilder().Use<TInterface, TController>(host, httpClientProvider);
        }
    }
}
