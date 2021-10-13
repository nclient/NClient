namespace NClient.Abstractions.Results
{
    public interface IResultBuilderProvider<TResponse>
    {
        IResultBuilder<TResponse> Create();
    }
}
