namespace NClient.Providers.Transport
{
    /// <summary>The provider that can create builder for transforming a transport response to NClient response.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IResponseBuilderProvider<TRequest, TResponse>
    {
        /// <summary>Creates builder for transforming a transport response to NClient response.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        IResponseBuilder<TRequest, TResponse> Create(IToolset toolset);
    }
}
