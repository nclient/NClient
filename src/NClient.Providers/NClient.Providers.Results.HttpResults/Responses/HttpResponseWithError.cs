// ReSharper disable once CheckNamespace

namespace NClient.Providers.Results.HttpResults
{
    public interface IHttpResponseWithError<TValue, TError> : IHttpResponseWithError<TError>, IHttpResponse<TValue>
    {
    }
    
    /// <summary>
    /// The container for HTTP response data with deserialized body including error.
    /// </summary>
    public class HttpResponseWithError<TData, TError> : HttpResponse<TData>, IHttpResponseWithError<TData, TError>
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
        /// <param name="data">The object obtained as a result of deserialization of the body.</param>
        /// <param name="error">The object obtained as a result of deserialization of the body if the IsSuccessful property for the HTTP response is false.</param>
        public HttpResponseWithError(IHttpResponse httpResponse, IHttpRequest httpRequest, TData? data, TError? error)
            : base(httpResponse, httpRequest, data)
        {
            Error = error;
        }

        /// <summary>
        /// Throws an exception if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        public new HttpResponseWithError<TData, TError> EnsureSuccess()
        {
            base.EnsureSuccess();
            return this;
        }
    }
    
    public interface IHttpResponseWithError<TError> : IHttpResponse
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        TError? Error { get; }
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
