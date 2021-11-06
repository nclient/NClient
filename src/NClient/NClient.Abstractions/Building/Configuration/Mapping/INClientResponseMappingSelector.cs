// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface INClientResponseMappingSelector<TRequest, TResponse>
    {
        INClientResponseMappingSetter<TRequest, TResponse> ForClient();
        INClientTransportResponseMappingSetter<TRequest, TResponse> ForTransport();
    }
}
