namespace NClient.Providers.Transport
{
    /// <summary>The provider for a component that can create transport.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface ITransportProvider<TRequest, TResponse>
    {
        /// <summary>Creates transport.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        ITransport<TRequest, TResponse> Create(IToolset toolset);
    }
}
