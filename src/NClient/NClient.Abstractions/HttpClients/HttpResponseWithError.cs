namespace NClient.Abstractions.HttpClients
{
    /// <summary>
    /// The container for HTTP response data with deserialized body including error.
    /// </summary>
    public class HttpResponseWithError<TValue, TError> : HttpResponse<TValue>, IHttpResponseWithError<TValue, TError>
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        public TError? Error { get; }

        /// <summary>
        /// Creates the container for HTTP response data.
        /// </summary>
        /// <param name="httpResponse">The HTTP response used as base HTTP response.</param>
        /// <param name="httpRequest">The HTTP request that the response belongs to.</param>
        /// <param name="value">The object obtained as a result of deserialization of the body.</param>
        /// <param name="error">The object obtained as a result of deserialization of the body if the IsSuccessful property for the HTTP response is false.</param>
        public HttpResponseWithError(HttpResponse httpResponse, HttpRequest httpRequest, TValue? value, TError? error)
            : base(httpResponse, httpRequest, value)
        {
            Error = error;
        }

        /// <summary>
        /// Throws an exception if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        public new HttpResponseWithError<TValue, TError> EnsureSuccess()
        {
            base.EnsureSuccess();
            return this;
        }
    }

    /// <summary>
    /// The container for HTTP response data with deserialized body error.
    /// </summary>
    public class HttpResponseWithError<TError> : HttpResponse, IHttpResponseWithError<TError>
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        public TError? Error { get; }

        /// <summary>
        /// Creates the container for HTTP response data.
        /// </summary>
        /// <param name="httpResponse">The HTTP response used as base HTTP response.</param>
        /// <param name="httpRequest">The HTTP request that the response belongs to.</param>
        /// <param name="error">The object obtained as a result of deserialization of the body if the IsSuccessful property for the HTTP response is false.</param>
        public HttpResponseWithError(IHttpResponse httpResponse, IHttpRequest httpRequest, TError? error)
            : base(httpResponse, httpRequest)
        {
            Error = error;
        }

        /// <summary>
        /// Throws an exception if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        public new HttpResponseWithError<TError> EnsureSuccess()
        {
            base.EnsureSuccess();
            return this;
        }
    }
}
