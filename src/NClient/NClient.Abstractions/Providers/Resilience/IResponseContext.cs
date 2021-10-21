namespace NClient.Abstractions.Providers.Resilience
{
    public interface IResponseContext<TRequest, TResponse>
    {
        TRequest Request { get; }
        TResponse Response { get; }
    }
}
