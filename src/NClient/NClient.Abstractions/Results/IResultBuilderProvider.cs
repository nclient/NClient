namespace NClient.Abstractions.Results
{
    public interface IResultBuilderProvider<TResponse>
    {
        IResultBuilder<TResponse> Create();
    }
    
    public interface IOrderedResultBuilderProvider
    {
        public int Order { get; }
    }
}
