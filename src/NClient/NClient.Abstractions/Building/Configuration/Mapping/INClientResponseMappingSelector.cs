// ReSharper disable once CheckNamespace

namespace NClient
{
    /// <summary>Selector for configuring mapping on the selected client layer.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface INClientResponseMappingSelector<TRequest, TResponse>
    {
        /// <summary>Select NClient layer.</summary>
        INClientResponseMappingSetter<TRequest, TResponse> ForClient();
        
        /// <summary>Select transport layer.</summary>
        INClientTransportResponseMappingSetter<TRequest, TResponse> ForTransport();
    }
}
