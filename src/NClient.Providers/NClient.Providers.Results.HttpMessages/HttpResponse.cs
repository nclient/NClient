using System;
using System.Net;
using NClient.Abstractions.HttpClients;
using NClient.Common.Helpers;

namespace NClient.Providers.Results.HttpMessages
{
    /// <summary>
    /// The container for HTTP response data with deserialized body.
    /// </summary>
    public class HttpResponse<TData> : HttpResponse, IHttpResponse<TData>
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body.
        /// </summary>
        public TData? Data { get; }

        /// <summary>
        /// Creates the container for HTTP response data.
        /// </summary>
        /// <param name="httpResponse">The HTTP response used as base HTTP response.</param>
        /// <param name="httpRequest">The HTTP request that the response belongs to.</param>
        /// <param name="data">The object obtained as a result of deserialization of the body.</param>
        public HttpResponse(IHttpResponse httpResponse, IHttpRequest httpRequest, TData? data)
            : base(httpResponse, httpRequest)
        {
            Data = data;
        }

        /// <summary>
        /// Throws an exception if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        public new HttpResponse<TData> EnsureSuccess()
        {
            base.EnsureSuccess();
            return this;
        }
    }

    /// <summary>
    /// The container for HTTP response data.
    /// </summary>
    public class HttpResponse : IHttpResponse
    {
        /// <summary>
        /// The HTTP request that the response belongs to.
        /// </summary>
        public IHttpRequest Request { get; }
        /// <summary>
        /// Gets string representation of response content.
        /// </summary>
        public IHttpResponseContent Content { get; set; }
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
        /// Gets headers returned by server with the response.
        /// </summary>
        public IHttpResponseHeaderContainer Headers { get; set; }
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
        public HttpResponse(IHttpRequest httpRequest)
        {
            Ensure.IsNotNull(httpRequest, nameof(httpRequest));

            Request = httpRequest;
            Content = new HttpResponseContent();
            Headers = new HttpResponseHeaderContainer(Array.Empty<IHttpHeader>());
        }

        internal HttpResponse(IHttpResponse httpResponse, IHttpRequest httpRequest) : this(httpRequest)
        {
            Ensure.IsNotNull(httpResponse, nameof(httpResponse));
            
            Content = httpResponse.Content;
            StatusCode = httpResponse.StatusCode;
            StatusDescription = httpResponse.StatusDescription;
            ResponseUri = httpResponse.ResponseUri;
            Headers = httpResponse.Headers;
            ErrorMessage = httpResponse.ErrorMessage;
            ErrorException = httpResponse.ErrorException;
            ProtocolVersion = httpResponse.ProtocolVersion;
        }

        /// <summary>
        /// Throws an exception if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        public IHttpResponse EnsureSuccess()
        {
            if (!IsSuccessful)
                throw ErrorException!;
            return this;
        }
    }
}
