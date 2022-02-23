using System.Threading;
using System.Threading.Tasks;

namespace NClient.Providers.Handling
{
    /// <summary>The handler that provides custom functionality to handling transport requests and responses.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IClientHandler<TRequest, TResponse>
    {
        /// <summary>Handles transport request before sending it.</summary>
        /// <param name="request">The source transport request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<TRequest> HandleRequestAsync(TRequest request, CancellationToken cancellationToken);

        /// <summary>Handles transport response after receiving it.</summary>
        /// <param name="response">The source transport response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<TResponse> HandleResponseAsync(TResponse response, CancellationToken cancellationToken);
    }

    /// <summary>The ordinal priority handler that provides custom functionality to handling transport requests and responses.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IOrderedClientHandler<TRequest, TResponse> : IClientHandler<TRequest, TResponse>
    {
        /// <summary>Gets the handler order. The order determines the order of handler execution.</summary>
        public int Order { get; }
    }
}
