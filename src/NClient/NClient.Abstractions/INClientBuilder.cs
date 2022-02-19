using System;

namespace NClient
{
    /// <summary>
    /// An builder abstraction used to create the client.
    /// </summary>
    public interface INClientBuilder
    {
        /// <summary>
        /// Sets the main client settings.
        /// </summary>
        /// <param name="baseUri">The base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface of controller used to create the client.</typeparam>
        INClientApiBuilder<TClient> For<TClient>(Uri baseUri)
            where TClient : class;
    }
}
