// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    public interface IResponseContext<TRequest, TResponse>
    {
        TRequest Request { get; }
        TResponse Response { get; }
    }
}
