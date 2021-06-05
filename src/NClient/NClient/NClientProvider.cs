using System;
using NClient.Abstractions;
using NClient.Common.Helpers;

namespace NClient
{
    /// <summary>
    /// The static provider used to create the client.
    /// </summary>
    public static class NClientProvider
    {
        /// <summary>
        /// Sets the main client settings.
        /// </summary>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TInterface">The type of interface of controller used to create the client.</typeparam>
        public static IOptionalNClientBuilder<TInterface> Use<TInterface>(string host)
            where TInterface : class
        {
            Ensure.IsNotNull(host, nameof(host));

            return new NClientBuilder().Use<TInterface>(host);
        }

        /// <summary>
        /// Sets the main client settings.
        /// </summary>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TInterface">The type of interface of controller used to create the client.</typeparam>
        /// <typeparam name="TController">The type of controller used to create the client.</typeparam>
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
