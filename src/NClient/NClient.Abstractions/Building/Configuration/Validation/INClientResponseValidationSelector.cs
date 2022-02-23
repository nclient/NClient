// ReSharper disable once CheckNamespace

namespace NClient
{
    /// <summary>Selector for configuring validation on the selected client layer.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface INClientResponseValidationSelector<TRequest, TResponse>
    {
        /// <summary>Select transport layer.</summary>
        INClientTransportResponseValidationSetter<TRequest, TResponse> ForTransport();
    }
}
