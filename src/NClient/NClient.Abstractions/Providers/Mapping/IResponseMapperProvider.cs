namespace NClient.Providers.Mapping
{
    public interface IResponseMapperProvider<TRequest, TResponse>
    {
        IResponseMapper<TRequest, TResponse> Create(IToolset toolset);
    }
    
    public interface IOrderedResponseMapperProvider
    {
        public int Order { get; }
    }
}
