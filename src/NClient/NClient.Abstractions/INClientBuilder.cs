using System;

namespace NClient.Abstractions
{
    /// <summary>
    /// An builder abstraction used to create the client.
    /// </summary>
    public interface INClientBuilder
    {
        /// <summary>
        /// Sets the main client settings.
        /// </summary>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TInterface">The type of interface of controller used to create the client.</typeparam>
        IOptionalNClientBuilder<TInterface> Use<TInterface>(string host)
            where TInterface : class;

        /// <summary>
        /// Sets the main client settings.
        /// </summary>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TInterface">The type of interface of controller used to create the client.</typeparam>
        /// <typeparam name="TController">The type of controller used to create the client.</typeparam>
        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        IOptionalNClientBuilder<TInterface> Use<TInterface, TController>(string host)
            where TInterface : class
            where TController : TInterface;
    }
}