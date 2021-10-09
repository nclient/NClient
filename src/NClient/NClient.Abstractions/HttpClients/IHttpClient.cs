using System.Threading.Tasks;

namespace NClient.Abstractions.HttpClients
{
    /// <summary>
    /// Invoker of HTTP requests.
    /// </summary>
    public interface IHttpClient<TRequest, TResponse>
    {
        /// <summary>
        /// Executes HTTP requests.
        /// </summary>
        /// <param name="httpRequest">The container for HTTP request data.</param>
        Task<TResponse> ExecuteAsync(TRequest httpRequest);
    }
}
