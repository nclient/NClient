using System.Threading.Tasks;

namespace NClient.Abstractions.HttpClients
{
    /// <summary>
    /// Invoker of HTTP requests.
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// Executes HTTP requests.
        /// </summary>
        /// <param name="request">The container for HTTP request data.</param>
        /// <param name="bodyType">The type of the deserialized response body.</param>
        /// <param name="errorType">The type of the deserialized response body if HTTP status code is not successful.</param>
        Task<HttpResponse> ExecuteAsync(HttpRequest request);
    }
}
