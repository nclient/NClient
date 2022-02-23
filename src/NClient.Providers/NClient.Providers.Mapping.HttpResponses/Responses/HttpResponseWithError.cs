// ReSharper disable once CheckNamespace

namespace NClient.Providers.Results.HttpResults
{
    /// <summary>The response containing an HTTP context with a deserialized response body including error.</summary>
    /// <typeparam name="TData">The type of the HTTP response body.</typeparam>
    /// <typeparam name="TError">The type of the HTTP response body in case of an error.</typeparam>
    public interface IHttpResponseWithError<TData, TError> : IHttpResponseWithError<TError>, IHttpResponse<TData>
    {
        /// <summary>Deconstructs response.</summary>
        /// <param name="data">The object obtained as a result of deserialization of the body. If the request was unsuccessful, the value will be null.</param>
        /// <param name="error">The object obtained as a result of deserialization of the body in case of an error. If the request was successful, the value will be null.</param>
        /// <param name="httpResponse">The response containing an HTTP context.</param>
        void Deconstruct(out TData? data, out TError? error, out IHttpResponse? httpResponse);
    }
    
    /// <summary>The response containing an HTTP context with a deserialized response body including error.</summary>
    /// <typeparam name="TData">The type of the HTTP response body.</typeparam>
    /// <typeparam name="TError">The type of the HTTP response body in case of an error.</typeparam>
    public class HttpResponseWithError<TData, TError> : HttpResponse<TData>, IHttpResponseWithError<TData, TError>
    {
        /// <summary>The object obtained as a result of deserialization of the body if the IsSuccessful property for the HTTP response is false.</summary>
        public TError? Error { get; }

        /// <summary>Initializes the container for HTTP response data.</summary>
        /// <param name="httpResponse">The HTTP response used as base HTTP response.</param>
        /// <param name="data">The object obtained as a result of deserialization of the body.</param>
        /// <param name="error">The object obtained as a result of deserialization of the body if the IsSuccessful property for the HTTP response is false.</param>
        public HttpResponseWithError(HttpResponse httpResponse, TData? data, TError? error)
            : base(httpResponse, data)
        {
            Error = error;
        }

        /// <summary>Throws an exception if the IsSuccessful property for the HTTP response is false.</summary>
        public new HttpResponseWithError<TData, TError> EnsureSuccess()
        {
            base.EnsureSuccess();
            return this;
        }

        /// <summary>Deconstructs response.</summary>
        /// <param name="data">The object obtained as a result of deserialization of the body. If the request was unsuccessful, the value will be null.</param>
        /// <param name="error">The object obtained as a result of deserialization of the body in case of an error. If the request was successful, the value will be null.</param>
        /// <param name="httpResponse">The response containing an HTTP context.</param>
        public void Deconstruct(out TData? data, out TError? error, out IHttpResponse? httpResponse)
        {
            data = Data;
            error = Error;
            httpResponse = this;
        }
        
        /// <summary>Deconstructs response.</summary>
        /// <param name="error">The object obtained as a result of deserialization of the body in case of an error. If the request was successful, the value will be null.</param>
        /// <param name="httpResponse">The response containing an HTTP context.</param>
        void IHttpResponseWithError<TError>.Deconstruct(out TError? error, out IHttpResponse httpResponse)
        {
            error = Error;
            httpResponse = this;
        }
    }
    
    /// <summary>The response containing an HTTP context with a deserialized response body in case of an error.</summary>
    /// <typeparam name="TError">The type of the HTTP response body in case of an error.</typeparam>
    public interface IHttpResponseWithError<TError> : IHttpResponse
    {
        /// <summary>The object obtained as a result of deserialization of the body if the IsSuccessful property for the HTTP response is false.</summary>
        TError? Error { get; }

        /// <summary>Deconstructs response.</summary>
        /// <param name="error">The object obtained as a result of deserialization of the body in case of an error. If the request was successful, the value will be null.</param>
        /// <param name="httpResponse">The response containing an HTTP context.</param>
        void Deconstruct(out TError? error, out IHttpResponse httpResponse);
    }

    /// <summary>The response containing an HTTP context with a deserialized response body in case of an error.</summary>
    /// <typeparam name="TError">The type of the HTTP response body in case of an error.</typeparam>
    public class HttpResponseWithError<TError> : HttpResponse, IHttpResponseWithError<TError>
    {
        /// <summary>The object obtained as a result of deserialization of the body if the IsSuccessful property for the HTTP response is false.</summary>
        public TError? Error { get; }

        /// <summary>Initializes the container for HTTP response data.</summary>
        /// <param name="httpResponse">The HTTP response used as base HTTP response.</param>
        /// <param name="error">The object obtained as a result of deserialization of the body if the IsSuccessful property for the HTTP response is false.</param>
        public HttpResponseWithError(HttpResponse httpResponse, TError? error)
            : base(httpResponse)
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

        /// <summary>Deconstructs response.</summary>
        /// <param name="error">The object obtained as a result of deserialization of the body in case of an error. If the request was successful, the value will be null.</param>
        /// <param name="httpResponse">The response containing an HTTP context.</param>
        public void Deconstruct(out TError? error, out IHttpResponse httpResponse)
        {
            error = Error;
            httpResponse = this;
        }
    }
}
