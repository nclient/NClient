// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface INClientHandlingSelector<TRequest, TResponse>
    {
        INClientTransportHandlingSetter<TRequest, TResponse> ForTransport();
    }
}
