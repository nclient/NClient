using System.Threading.Tasks;

namespace NClient.Providers.Handling
{
    /// <summary>
    /// Provides custom functionality to handling transport requests and responses.
    /// </summary>
    public interface IClientHandler<TRequest, TResponse>
    {
        /// <summary>
        /// Handles transport request before sending it.
        /// </summary>
        /// <param name="request">The source transport request.</param>
        /// <returns></returns>
        Task<TRequest> HandleRequestAsync(TRequest request);

        /// <summary>
        /// Handles transport response after receiving it.
        /// </summary>
        /// <param name="response">The source transport response.</param>
        /// <returns></returns>
        Task<TResponse> HandleResponseAsync(TResponse response);
    }

    public interface IOrderedClientHandler
    {
        public int Order { get; }
    }
}
