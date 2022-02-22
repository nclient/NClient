namespace NClient.Providers.Transport
{
    /// <summary>A provider abstraction for a component that can create <see cref="ITransport{TRequest,TResponse}"/> instances.</summary>
    public interface ITransportProvider<TRequest, TResponse>
    {
        ITransport<TRequest, TResponse> Create(IToolset toolset);
    }
}
