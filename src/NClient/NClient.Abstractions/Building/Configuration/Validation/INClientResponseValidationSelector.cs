// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface INClientResponseValidationSelector<TRequest, TResponse>
    {
        INClientTransportResponseValidationSetter<TRequest, TResponse> ForTransport();
    }
}
