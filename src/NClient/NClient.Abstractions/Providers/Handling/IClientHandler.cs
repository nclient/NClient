using System.Threading;
using System.Threading.Tasks;

namespace NClient.Providers.Handling
{
    /// <summary>Provides custom functionality to handling transport requests and responses.</summary>
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

    public interface IOrderedClientHandler
    {
        public int Order { get; }
    }
}
