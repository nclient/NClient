// ReSharper disable once CheckNamespace

namespace NClient.Providers.Resilience
{
    public interface IResponseContext<TRequest, TResponse>
    {
        TRequest Request { get; }
        TResponse Response { get; }
    }
}
