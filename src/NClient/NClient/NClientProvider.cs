using System;
using NClient.Abstractions;
using NClient.Common.Helpers;

namespace NClient
{
    public static class NClientProvider
    {
        public static IOptionalNClientBuilder<TInterface> Use<TInterface>(string host)
            where TInterface : class
        {
            Ensure.IsNotNull(host, nameof(host));

            return new NClientBuilder().Use<TInterface>(host);
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public static IOptionalNClientBuilder<TInterface> Use<TInterface, TController>(string host)
            where TInterface : class
            where TController : TInterface
        {
            Ensure.IsNotNull(host, nameof(host));

            return new NClientBuilder().Use<TInterface, TController>(host);
        }
    }
}
