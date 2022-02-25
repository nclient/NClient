namespace NClient.Providers.Mapping
{
    /// <summary>The provider of a mapper that converts transport responses into custom results.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IResponseMapperProvider<TRequest, TResponse>
    {
        /// <summary>Creates the mapper that converts transport responses into custom results.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        IResponseMapper<TRequest, TResponse> Create(IToolset toolset);
    }
    
    /// <summary>The ordinal priority provider of a mapper that converts transport responses into custom results.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IOrderedResponseMapperProvider<TRequest, TResponse> : IResponseMapperProvider<TRequest, TResponse>
    {
        /// <summary>Gets the mapper order. The order determines the order of mapper execution.</summary>
        public int Order { get; }
    }
}
