using System.Threading.Tasks;

namespace NClient.Providers.Transport
{
    /// <summary>
    /// Invoker of requests.
    /// </summary>
    public interface ITransport<TRequest, TResponse>
    {
        /// <summary>
        /// Executes requests.
        /// </summary>
        /// <param name="request">The container for request data.</param>
        Task<TResponse> ExecuteAsync(TRequest request);
    }
}
