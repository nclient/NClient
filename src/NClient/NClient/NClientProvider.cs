using System;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Standalone;
using NClient.Standalone.ControllerBasedClients;
using NClient.Standalone.InterfaceBasedClients;
using RestSharp.Authenticators;

namespace NClient
{
    public static class NClientProvider
    {
        public static IInterfaceBasedClientBuilder<T> Use<T>(string host) where T : class
        {
            return new NClientBuilder().Use<T>(host, new RestSharpHttpClientProvider());
        }

        public static IInterfaceBasedClientBuilder<T> Use<T>(string host, IAuthenticator authenticator) where T : class
        {
            return new NClientBuilder().Use<T>(host, new RestSharpHttpClientProvider(authenticator));
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(
            string host)
            where TInterface : class
            where TController : TInterface
        {
            return new NClientBuilder().Use<TInterface, TController>(host, new RestSharpHttpClientProvider());
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(
            string host, IAuthenticator authenticator)
            where TInterface : class
            where TController : TInterface
        {
            return new NClientBuilder().Use<TInterface, TController>(host, new RestSharpHttpClientProvider(authenticator));
        }
    }
}
