using System;
using System.Net;
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
        /// Gets MIME content type of response.
        /// </summary>
        public string? ContentType { get; }
        /// <summary>
        /// Gets the length in bytes of the response content.
        /// </summary>
        public long? ContentLength { get; }
        /// <summary>
        /// Gets encoding of the response content.
        /// </summary>
        public string? ContentEncoding { get; }
        /// <summary>
        /// Gets string representation of response content.
        /// </summary>
        public string? Content { get; }
        /// <summary>
        /// Gets HTTP response status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; }
        /// <summary>
        /// Gets description of HTTP status returned.
        /// </summary>
        public string? StatusDescription { get; }
        /// <summary>
        /// Gets the URL that actually responded to the content (different from request if redirected).
        /// </summary>
        public Uri? ResponseUri { get; }
        /// <summary>
        /// Gets HttpWebResponse.Server.
        /// </summary>
        public string? Server { get; }
        /// <summary>
        /// Gets headers returned by server with the response.
        /// </summary>
        public HttpHeader[]? Headers { get; }
        /// <summary>
        /// Gets HTTP error generated while attempting request.
        /// </summary>

        public ClientHttpRequestException(HttpResponse httpResponse) : base(httpResponse.ErrorMessage!)
        {
            Request = httpResponse.Request;
            ContentType = httpResponse.ContentType;
            ContentLength = httpResponse.ContentLength;
            ContentEncoding = httpResponse.ContentEncoding;
            Content = httpResponse.Content;
            StatusCode = httpResponse.StatusCode;
            StatusDescription = httpResponse.StatusDescription;
            ResponseUri = httpResponse.ResponseUri;
            Server = httpResponse.Server;
            Headers = httpResponse.Headers;
        }
    }
}
