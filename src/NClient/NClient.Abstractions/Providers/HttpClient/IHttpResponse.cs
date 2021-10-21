using System;
using System.Net;

namespace NClient.Abstractions.Providers.HttpClient
{
    public interface IHttpResponse<TValue> : IHttpResponse
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body.
        /// </summary>
        TValue? Data { get; }
    }
    
    public interface IHttpResponse
    {
        /// <summary>
        /// The HTTP request that the response belongs to.
        /// </summary>
        IHttpRequest Request { get; }
        /// <summary>
        /// Gets string representation of response content.
        /// </summary>
        IHttpResponseContent Content { get; set; }
        /// <summary>
        /// Gets HTTP response status code.
        /// </summary>
        HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// Gets description of HTTP status returned.
        /// </summary>
        string? StatusDescription { get; set; }
        /// <summary>
        /// Gets the URL that actually responded to the content (different from request if redirected).
        /// </summary>
        Uri? ResponseUri { get; set; }
        /// <summary>
        /// Gets headers returned by server with the response.
        /// </summary>
        IHttpResponseHeaderContainer Headers { get; set; }
        /// <summary>
        /// Gets HTTP error generated while attempting request.
        /// </summary>
        string? ErrorMessage { get; set; }
        /// <summary>
        /// Gets the exception thrown when error is encountered.
        /// </summary>
        Exception? ErrorException { get; set; }
        /// <summary>
        /// Gets the HTTP protocol version (1.0, 1.1, etc).
        /// </summary>
        Version? ProtocolVersion { get; set; }
        /// <summary>
        /// Gets information about the success of the request.
        /// </summary>
        bool IsSuccessful { get; }
        /// <summary>
        /// Throws an exception if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        IHttpResponse EnsureSuccess();
    }
}
