using System.Threading.Tasks;
using NClient.Abstractions.Invocation;

namespace NClient.Abstractions.Handling
{
    /// <summary>
    /// Provides custom functionality to handling HTTP requests and responses.
    /// </summary>
    public interface IClientHandler<TRequest, TResponse>
    {
        /// <summary>
        /// Handles HTTP request before sending it.
        /// </summary>
        /// <param name="request">The source HTTP request.</param>
        /// <param name="methodInvocation">The information about invocation of a client method.</param>
        /// <returns></returns>
        Task<TRequest> HandleRequestAsync(TRequest request, IMethodInvocation methodInvocation);

        /// <summary>
        /// Handles HTTP response after receiving it.
        /// </summary>
        /// <param name="response">The source HTTP response.</param>
        /// <param name="methodInvocation">The information about invocation of a client method.</param>
        /// <returns></returns>
        Task<TResponse> HandleResponseAsync(TResponse response, IMethodInvocation methodInvocation);
    }
}
