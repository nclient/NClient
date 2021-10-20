using System.Threading.Tasks;

namespace NClient.Abstractions.Handling
{
    // TODO: HttpRequest support
    /// <summary>
    /// Provides custom functionality to handling HTTP requests and responses.
    /// </summary>
    public interface IClientHandler<TRequest, TResponse>
    {
        /// <summary>
        /// Handles HTTP request before sending it.
        /// </summary>
        /// <param name="request">The source HTTP request.</param>
        /// <returns></returns>
        Task<TRequest> HandleRequestAsync(TRequest request);

        /// <summary>
        /// Handles HTTP response after receiving it.
        /// </summary>
        /// <param name="response">The source HTTP response.</param>
        /// <returns></returns>
        Task<TResponse> HandleResponseAsync(TResponse response);
    }

    public interface IOrderedClientHandler
    {
        public int Order { get; }
    }
}
