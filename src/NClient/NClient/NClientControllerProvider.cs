using System;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Standalone;
using RestSharp.Authenticators;

namespace NClient
{
    [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use NClientProvider.")]
    public static class NClientControllerProvider
    {
        public static INClientControllerBuilder<TInterface, TController> Use<TInterface, TController>(
            string host)
            where TInterface : class
            where TController : TInterface
        {
            return new NClientControllerBuilder().Use<TInterface, TController>(host, new RestSharpHttpClientProvider());
        }

        public static INClientControllerBuilder<TInterface, TController> Use<TInterface, TController>(
            string host, IAuthenticator authenticator)
            where TInterface : class
            where TController : TInterface
        {
            return new NClientControllerBuilder().Use<TInterface, TController>(host, new RestSharpHttpClientProvider(authenticator));
        }
    }
}
