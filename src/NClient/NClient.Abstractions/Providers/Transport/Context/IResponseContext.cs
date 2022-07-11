﻿// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    /// <summary>The context containing transport request and response.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IResponseContext<TRequest, TResponse>
    {
        /// <summary>Gets transport request.</summary>
        TRequest Request { get; }

        /// <summary>Gets transport response.</summary>
        TResponse Response { get; }
    }
}
