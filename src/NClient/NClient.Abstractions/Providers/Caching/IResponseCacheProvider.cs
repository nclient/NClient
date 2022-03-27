namespace NClient.Providers.Caching
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="IResponseCacheWorker"/> instances.
    /// </summary>
    public interface IResponseCacheProvider
    {
        /// <summary>Creates and configures an instance of <see cref="IResponseCacheWorker"/> instance.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        IResponseCacheWorker Create(IToolset toolset);
    }
}
