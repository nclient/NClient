using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Results.HttpResults
{
    public interface IHttpResponse<TValue> : IHttpResponse
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body.
        /// </summary>
        TValue? Data { get; }
    }
    
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
        public HttpResponse(HttpResponse httpResponse, TData? data)
            : base(httpResponse)
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
        
        /// <summary>
        /// Deconstruct object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="httpResponse"></param>
        public void Deconstruct(out TData? value, out IHttpResponse? httpResponse)
        {
            value = Data;
            httpResponse = this;
        }
    }

    public interface IHttpResponse
    {
        IHttpRequest Request { get; }
        Version Version { get; }
        HttpContent Content { get; }
        HttpStatusCode StatusCode { get; }
        string ReasonPhrase { get; }
        HttpResponseHeaders Headers { get; }
        #if !NETSTANDARD2_0
        HttpResponseHeaders TrailingHeaders { get; }
        #endif
        /// <summary>
        /// Gets HTTP error generated while attempting request.
        /// </summary>
        string? ErrorMessage { get; set; }
        /// <summary>
        /// Gets the exception thrown when error is encountered.
        /// </summary>
        Exception? ErrorException { get; set; }
        /// <summary>
        /// Gets information about the success of the request.
        /// </summary>
        bool IsSuccessful { get; }
        /// <summary>
        /// Throws an exception if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        IHttpResponse EnsureSuccess();
    }
    
    /// <summary>
    /// The container for HTTP response data.
    /// </summary>
    public class HttpResponse : IHttpResponse
    {
        private readonly HttpResponseMessage _httpResponseMessage;

        public IHttpRequest Request { get; }

        public Version Version => _httpResponseMessage.Version;
        public HttpContent Content => _httpResponseMessage.Content;
        public HttpStatusCode StatusCode => _httpResponseMessage.StatusCode;
        public string ReasonPhrase => _httpResponseMessage.ReasonPhrase;
        public HttpResponseHeaders Headers => _httpResponseMessage.Headers;

        #if !NETSTANDARD2_0
        public HttpResponseHeaders TrailingHeaders => _httpResponseMessage.TrailingHeaders;
        #endif
        
        /// <summary>
        /// Gets HTTP error generated while attempting request.
        /// </summary>
        public string? ErrorMessage { get; set; }
        /// <summary>
        /// Gets the exception thrown when error is encountered.
        /// </summary>
        public Exception? ErrorException { get; set; }
       
        /// <summary>
        /// Gets information about the success of the request.
        /// </summary>
        public bool IsSuccessful => (int) StatusCode >= 200 && (int) StatusCode <= 299;

        /// <summary>
        /// Creates the container for HTTP response data.
        /// </summary>
        /// <param name="httpRequest">The HTTP request that the response belongs to.</param>
        /// <param name="httpResponseMessage">The HTTP response message that the response belongs to.</param>
        public HttpResponse(
            IHttpRequest httpRequest, 
            HttpResponseMessage httpResponseMessage)
        {
            Request = httpRequest;
            
            _httpResponseMessage = httpResponseMessage;
        }
        
        public HttpResponse(
            IHttpRequest httpRequest, 
            HttpResponseMessage httpResponseMessage,
            string errorMessage,
            Exception errorException)
        {
            Request = httpRequest;
            ErrorMessage = errorMessage;
            ErrorException = errorException;
            
            _httpResponseMessage = httpResponseMessage;
        }
        
        internal HttpResponse(HttpResponse httpResponse)
        {
            Request = httpResponse.Request;
            ErrorMessage = httpResponse.ErrorMessage;
            ErrorException = httpResponse.ErrorException;
            
            _httpResponseMessage = httpResponse._httpResponseMessage;
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
