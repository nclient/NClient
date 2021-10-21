namespace NClient.Abstractions.Providers.Handling
{
    public interface IClientHandlerProvider<TRequest, TResponse>
    {
        IClientHandler<TRequest, TResponse> Create();
    }
}
