using System;
using System.Net;
using System.Text;
using NClient.Common.Helpers;

namespace NClient.Abstractions.HttpClients
{
    /// <summary>
    /// The container for HTTP response data with deserialized body.
    /// </summary>
    public class HttpResponse<TValue> : HttpResponse
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body.
        /// </summary>
        public TValue? Value { get; }

        /// <summary>
        /// Creates the container for HTTP response data.
        /// </summary>
        /// <param name="httpResponse">The HTTP response used as base HTTP response.</param>
        /// <param name="httpRequest">The HTTP request that the response belongs to.</param>
        /// <param name="value">The object obtained as a result of deserialization of the body.</param>
        public HttpResponse(HttpResponse httpResponse, HttpRequest httpRequest, TValue? value)
            : base(httpResponse, httpRequest)
        {
            Value = value;
        }

        /// <summary>
        /// Throws an exception if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        public new HttpResponse<TValue> EnsureSuccess()
        {
            base.EnsureSuccess();
            return this;
        }
    }

    /// <summary>
    /// The container for HTTP response data.
    /// </summary>
    public class HttpResponse
    {
        /// <summary>
        /// The HTTP request that the response belongs to.
        /// </summary>
        public HttpRequest Request { get; }
        /// <summary>
        /// Gets MIME content type of response.
        /// </summary>
        public string? ContentType { get; set; }
        /// <summary>
        /// Gets the length in bytes of the response content.
        /// </summary>
        public long? ContentLength { get; set; }
        /// <summary>
        /// Gets encoding of the response content.
        /// </summary>
        public string? ContentEncoding { get; set; }
        /// <summary>
        /// Gets string representation of response content.
        /// </summary>
        public string? Content => AsString(RawBytes, ContentEncoding);
        /// <summary>
        /// Response content
        /// </summary>
        public byte[]? RawBytes { get; set; }
        /// <summary>
        /// Gets HTTP response status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// Gets description of HTTP status returned.
        /// </summary>
        public string? StatusDescription { get; set; }
        /// <summary>
        /// Gets the URL that actually responded to the content (different from request if redirected).
        /// </summary>
        public Uri? ResponseUri { get; set; }
        /// <summary>
        /// Gets HttpWebResponse.Server.
        /// </summary>
        public string? Server { get; set; }
        /// <summary>
        /// Gets headers returned by server with the response.
        /// </summary>
        public HttpHeader[]? Headers { get; set; }
        /// <summary>
        /// Gets HTTP error generated while attempting request.
        /// </summary>
        public string? ErrorMessage { get; set; }
        /// <summary>
        /// Gets the exception thrown when error is encountered.
        /// </summary>
        public Exception? ErrorException { get; set; }
        /// <summary>
        /// Gets the HTTP protocol version (1.0, 1.1, etc).
        /// </summary>
        public Version? ProtocolVersion { get; set; }

        /// <summary>
        /// Gets information about the success of the request.
        /// </summary>
        public bool IsSuccessful => (int)StatusCode >= 200 && (int)StatusCode <= 299;

        /// <summary>
        /// Creates the container for HTTP response data.
        /// </summary>
        /// <param name="httpRequest">The HTTP request that the response belongs to.</param>
        public HttpResponse(HttpRequest httpRequest)
        {
            Ensure.IsNotNull(httpRequest, nameof(httpRequest));

            Request = httpRequest;
        }

        internal HttpResponse(HttpResponse httpResponse, HttpRequest httpRequest) : this(httpRequest)
        {
            Ensure.IsNotNull(httpResponse, nameof(httpResponse));

            ContentType = httpResponse.ContentType;
            ContentLength = httpResponse.ContentLength;
            ContentEncoding = httpResponse.ContentEncoding;
            RawBytes = httpResponse.RawBytes;
            StatusCode = httpResponse.StatusCode;
            StatusDescription = httpResponse.StatusDescription;
            ResponseUri = httpResponse.ResponseUri;
            Server = httpResponse.Server;
            Headers = httpResponse.Headers;
            ErrorMessage = httpResponse.ErrorMessage;
            ErrorException = httpResponse.ErrorException;
            ProtocolVersion = httpResponse.ProtocolVersion;
        }

        /// <summary>
        /// Throws an exception if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        public HttpResponse EnsureSuccess()
        {
            if (!IsSuccessful)
                throw ErrorException!;
            return this;
        }

        private static string? AsString(byte[]? bytes, string? encodingName)
        {
            if (string.IsNullOrEmpty(encodingName))
                return AsString(bytes, Encoding.UTF8);

            try
            {
                var encoding = Encoding.GetEncoding(encodingName);
                return AsString(bytes, encoding);
            }
            catch (ArgumentException)
            {
                return AsString(bytes, Encoding.UTF8);
            }
        }

        private static string? AsString(byte[]? buffer, Encoding encoding)
        {
            return buffer == null ? null : encoding.GetString(buffer, 0, buffer.Length);
        }
    }
}
