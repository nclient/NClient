using System;
using NClient.Abstractions;

namespace NClient
{
    public static class NClientProvider
    {
        public static IInterfaceBasedClientBuilder<TInterface> Use<TInterface>(string host) where TInterface : class
        {
            return new NClientBuilder().Use<TInterface>(host);
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(string host)
            where TInterface : class
            where TController : TInterface
        {
            return new NClientBuilder().Use<TInterface, TController>(host);
        }
    }
}
