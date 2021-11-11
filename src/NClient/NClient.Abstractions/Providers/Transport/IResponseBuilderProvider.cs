namespace NClient.Providers.Transport
{
    public interface IResponseBuilderProvider<TRequest, TResponse>
    {
        IResponseBuilder<TRequest, TResponse> Create(IToolSet toolset);
    }
}
