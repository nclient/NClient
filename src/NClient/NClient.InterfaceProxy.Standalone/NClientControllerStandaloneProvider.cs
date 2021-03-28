using System;
using NClient.Abstractions.HttpClients;

namespace NClient.AspNetProxy
{
    [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use NClientStandaloneProvider.")]
    public static class NClientControllerStandaloneProvider
    {
        public static INClientControllerBuilder<TInterface, TController> Use<TInterface, TController>(
            string host, IHttpClientProvider httpClientProvider)
            where TInterface : class
            where TController : TInterface
        {
            return new NClientControllerBuilder().Use<TInterface, TController>(host, httpClientProvider);
        }
    }
}
