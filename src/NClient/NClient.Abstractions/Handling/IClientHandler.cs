using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;

namespace NClient.Abstractions.Handling
{
    /// <summary>
    /// Provides custom functionality to handling HTTP requests and responses.
    /// </summary>
    public interface IClientHandler
    {
        /// <summary>
        /// Handles HTTP request before sending it.
        /// </summary>
        /// <param name="httpRequest">The source HTTP request.</param>
        /// <param name="methodInvocation">The information about invocation of a client method.</param>
        /// <returns></returns>
        Task<HttpRequest> HandleRequestAsync(HttpRequest httpRequest, MethodInvocation methodInvocation);

        /// <summary>
        /// Handles HTTP response after receiving it.
        /// </summary>
        /// <param name="httpResponse">The source HTTP response.</param>
        /// <param name="methodInvocation">The information about invocation of a client method.</param>
        /// <returns></returns>
        Task<HttpResponse> HandleResponseAsync(HttpResponse httpResponse, MethodInvocation methodInvocation);
    }
}