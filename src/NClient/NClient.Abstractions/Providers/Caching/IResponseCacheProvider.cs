namespace NClient.Providers.Caching
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="IResponseCacheWorker{TRequest,TResponse}"/> instances.
    /// </summary>
    public interface IResponseCacheProvider<TRequest, TResponse>
    {
        /// <summary>Creates and configures an instance of <see cref="IResponseCacheWorker{TRequest,TResponse}"/> instance.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        IResponseCacheWorker<TRequest, TResponse> Create(IToolset toolset);
    }
}
