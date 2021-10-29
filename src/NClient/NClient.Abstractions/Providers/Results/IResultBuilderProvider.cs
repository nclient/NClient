// ReSharper disable once CheckNamespace

namespace NClient.Providers.Results
{
    public interface IResultBuilderProvider<TRequest, TResponse>
    {
        IResultBuilder<TRequest, TResponse> Create();
    }
    
    public interface IOrderedResultBuilderProvider
    {
        public int Order { get; }
    }
}
