// ReSharper disable once CheckNamespace

namespace NClient.Providers.Handling
{
    /// <summary>The providers creating handlers that provide custom functionality to handling transport requests and responses.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IClientHandlerProvider<TRequest, TResponse>
    {
        /// <summary>Creates the providers creating handlers that provide custom functionality to handling transport requests and responses.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        IClientHandler<TRequest, TResponse> Create(IToolset toolset);
    }
}
