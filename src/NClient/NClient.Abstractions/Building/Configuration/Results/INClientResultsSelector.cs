// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface INClientResultsSelector<TRequest, TResponse>
    {
        INClientResultsSetter<TRequest, TResponse> ForClient();
        INClientTransportResultsSetter<TRequest, TResponse> ForTransport();
    }
}
