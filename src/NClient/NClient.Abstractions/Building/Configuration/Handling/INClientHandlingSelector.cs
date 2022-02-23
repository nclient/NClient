// ReSharper disable once CheckNamespace

namespace NClient
{
    /// <summary>Selector for configuring handling on the selected client layer.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface INClientHandlingSelector<TRequest, TResponse>
    {
        /// <summary>Select transport layer.</summary>
        INClientTransportHandlingSetter<TRequest, TResponse> ForTransport();
    }
}
