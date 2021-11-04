﻿namespace NClient
{
    /// <summary>
    /// An builder abstraction used to create the client.
    /// </summary>
    public interface INClientAdvancedBuilder
    {
        /// <summary>
        /// Sets the main client settings.
        /// </summary>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface of controller used to create the client.</typeparam>
        INClientAdvancedApiBuilder<TClient> For<TClient>(string host)
            where TClient : class;
    }
}