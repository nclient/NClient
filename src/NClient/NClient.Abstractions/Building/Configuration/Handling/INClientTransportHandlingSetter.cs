using System.Collections.Generic;
using NClient.Providers.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>Setter for custom functionality to handling transport requests and responses.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface INClientTransportHandlingSetter<TRequest, TResponse>
    {
        /// <summary>Sets settings for custom functionality to handling transport requests and responses.</summary>
        /// <param name="settings">The settings for handling operations that handles the transport messages.</param>
        INClientHandlingSelector<TRequest, TResponse> Use(IEnumerable<IClientHandlerSettings<TRequest, TResponse>> settings);

        /// <summary>Sets handlers for custom functionality to handling transport requests and responses.</summary>
        /// <param name="handlers">The handlers that provide custom functionality to handling transport requests and responses.</param>
        INClientHandlingSelector<TRequest, TResponse> Use(IEnumerable<IClientHandler<TRequest, TResponse>> handlers);

        /// <summary>Sets handler providers for custom functionality to handling transport requests and responses.</summary>
        /// <param name="providers">The providers creating handlers that provide custom functionality to handling transport requests and responses.</param>
        INClientHandlingSelector<TRequest, TResponse> Use(IEnumerable<IClientHandlerProvider<TRequest, TResponse>> providers);
    }
}
