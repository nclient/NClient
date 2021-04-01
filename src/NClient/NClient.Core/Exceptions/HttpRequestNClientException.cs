using System.Net;

namespace NClient.Core.Exceptions
{
    public class HttpRequestNClientException : NClientException
    {
        public HttpRequestNClientException(HttpStatusCode httpStatusCode, string? errorMessage)
            : base($"The request completed with status {httpStatusCode} (http code: {(int)httpStatusCode}). Error message: {errorMessage ?? ""}")
        {

        }
    }
}
