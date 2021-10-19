namespace NClient.Abstractions.Handling
{
    public interface IClientHandlerProvider<TRequest, TResponse>
    {
        IClientHandler<TRequest, TResponse> Create();
    }
}
