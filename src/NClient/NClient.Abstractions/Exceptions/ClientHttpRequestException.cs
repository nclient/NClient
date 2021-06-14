using NClient.Abstractions.HttpClients;

namespace NClient.Abstractions.Exceptions
{
    /// <summary>
    /// Represents exceptions thrown by NClient client during the processing of the HTTP request.
    /// </summary>
    public class ClientHttpRequestException : NClientException
    {
        /// <summary>
        /// The HTTP request that the response belongs to.
        /// </summary>
        public HttpRequest Request { get; }
        
        /// <summary>
        /// The HTTP response.
        /// </summary>
        public HttpResponse Response { get; }

        public ClientHttpRequestException(HttpResponse httpResponse) : base(BuildMessage(httpResponse))
        {
            Request = httpResponse.Request;
            Response = httpResponse;
        }

        private static string BuildMessage(HttpResponse httpResponse)
        {
            return $"{httpResponse.ErrorMessage}\nResponse content: {httpResponse.Content}";
        }
    }
}
