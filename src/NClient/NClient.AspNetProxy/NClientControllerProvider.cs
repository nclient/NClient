using System;
using Microsoft.AspNetCore.Mvc;
using NClient.Abstractions.Clients;
using NClient.Providers.HttpClient.RestSharp;
using RestSharp.Authenticators;

namespace NClient.AspNetProxy
{
    [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use NClientProvider.")]
    public static class NClientControllerProvider
    {
        public static INClientControllerBuilder<TInterface, TController> Use<TInterface, TController>(
            string host)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return new NClientControllerBuilder().Use<TInterface, TController>(host, new RestSharpHttpClientProvider());
        }

        public static INClientControllerBuilder<TInterface, TController> Use<TInterface, TController>(
            string host, IAuthenticator authenticator)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return new NClientControllerBuilder().Use<TInterface, TController>(host, new RestSharpHttpClientProvider(authenticator));
        }
    }
}
