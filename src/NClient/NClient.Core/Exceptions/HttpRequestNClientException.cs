using System.Net;

namespace NClient.Core.Exceptions
{
    public class HttpRequestNClientException : RequestNClientException
    {
        public HttpStatusCode HttpStatusCode { get; }

        public HttpRequestNClientException(HttpStatusCode httpStatusCode, string? errorMessage)
            : base($"The request completed with status {httpStatusCode} (http code: {(int)httpStatusCode}). Error message: {errorMessage ?? ""}")
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
