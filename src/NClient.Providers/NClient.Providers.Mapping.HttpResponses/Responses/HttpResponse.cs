using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Results.HttpResults
{
    /// <summary>The response containing an HTTP context with a deserialized response body.</summary>
    /// <typeparam name="TData">The type of the HTTP response body.</typeparam>
    public interface IHttpResponse<TData> : IHttpResponse
    {
        /// <summary>Gets the object obtained as a result of deserialization of the body. If the request was unsuccessful, the value will be null.</summary>
        TData? Data { get; }

        /// <summary>Deconstructs response.</summary>
        /// <param name="data">The object obtained as a result of deserialization of the body. If the request was unsuccessful, the value will be null.</param>
        /// <param name="httpResponse">The response containing an HTTP context.</param>
        void Deconstruct(out TData? data, out IHttpResponse httpResponse);
    }
    
    /// <summary>The response containing an HTTP context with a deserialized response body.</summary>
    /// <typeparam name="TData">The type of the HTTP response body.</typeparam>
    public class HttpResponse<TData> : HttpResponse, IHttpResponse<TData>
    {
        /// <summary>Gets the object obtained as a result of deserialization of the body. If the request was unsuccessful, the value will be null.</summary>
        public TData? Data { get; }

        /// <summary>Creates the container for HTTP response with deserialized body.</summary>
        /// <param name="httpResponse">The response containing an HTTP context.</param>
        /// <param name="data">The object obtained as a result of deserialization of the body.</param>
        public HttpResponse(HttpResponse httpResponse, TData? data)
            : base(httpResponse)
        {
            Data = data;
        }

        /// <summary>Throws an exception if the IsSuccessful property for the HTTP response is false.</summary>
        public new HttpResponse<TData> EnsureSuccess()
        {
            base.EnsureSuccess();
            return this;
        }
        
        /// <summary>Deconstructs response.</summary>
        /// <param name="data">The object obtained as a result of deserialization of the body. If the request was unsuccessful, the value will be null.</param>
        /// <param name="httpResponse">The response containing an HTTP context.</param>
        public void Deconstruct(out TData? data, out IHttpResponse httpResponse)
        {
            data = Data;
            httpResponse = this;
        }
    }

    /// <summary>The response containing an HTTP context.</summary>
    public interface IHttpResponse
    {
        /// <summary>Gets the request containing an HTTP context.</summary>
        IHttpRequest Request { get; }
        
        /// <summary>Gets the HTTP response version. The default is 1.1.</summary>
        Version Version { get; }
        
        /// <summary>Gets the content of a HTTP response.</summary>
        HttpContent Content { get; }
        
        /// <summary>Gets the status code of the HTTP response.</summary>
        HttpStatusCode StatusCode { get; }
        
        /// <summary>Gets the reason phrase which typically is sent by servers together with the status code.</summary>
        string ReasonPhrase { get; }
        
        /// <summary>Gets the collection of HTTP response headers.</summary>
        HttpResponseHeaders Headers { get; }
        
        #if !NETSTANDARD2_0
        /// <summary>Gets the collection of trailing headers included in an HTTP response.</summary>
        /// <exception cref="T:System.Net.Http.HttpRequestException">PROTOCOL_ERROR: The HTTP/2 response contains pseudo-headers in the Trailing Headers Frame.</exception>
        HttpResponseHeaders TrailingHeaders { get; }
        #endif
        
        /// <summary>Gets HTTP error generated while attempting request.</summary>
        string? ErrorMessage { get; set; }
        
        /// <summary>Gets the exception thrown when error is encountered.</summary>
        Exception? ErrorException { get; set; }
        
        /// <summary>Gets information about the success of the request.</summary>
        bool IsSuccessful { get; }
        
        /// <summary>Throws an exception if the IsSuccessful property for the HTTP response is false.</summary>
        IHttpResponse EnsureSuccess();
    }
    
    /// <summary>The response containing an HTTP context..</summary>
    public class HttpResponse : IHttpResponse
    {
        private readonly HttpResponseMessage _httpResponseMessage;
        
        /// <summary>Gets the request containing an HTTP context.</summary>
        public IHttpRequest Request { get; }

        /// <summary>Gets the HTTP response version. The default is 1.1.</summary>
        public Version Version => _httpResponseMessage.Version;
        
        /// <summary>Gets the content of a HTTP response.</summary>
        public HttpContent Content => _httpResponseMessage.Content;
        
        /// <summary>Gets the status code of the HTTP response.</summary>
        public HttpStatusCode StatusCode => _httpResponseMessage.StatusCode;
        
        /// <summary>Gets the reason phrase which typically is sent by servers together with the status code.</summary>
        public string ReasonPhrase => _httpResponseMessage.ReasonPhrase;
       
        /// <summary>Gets the collection of HTTP response headers.</summary>
        public HttpResponseHeaders Headers => _httpResponseMessage.Headers;

        #if !NETSTANDARD2_0
        /// <summary>Gets the collection of trailing headers included in an HTTP response.</summary>
        /// <exception cref="T:System.Net.Http.HttpRequestException">PROTOCOL_ERROR: The HTTP/2 response contains pseudo-headers in the Trailing Headers Frame.</exception>
        public HttpResponseHeaders TrailingHeaders => _httpResponseMessage.TrailingHeaders;
        #endif
        
        /// <summary>Gets or sets HTTP error generated while attempting request.</summary>
        public string? ErrorMessage { get; set; }
        
        /// <summary>Gets or sets the exception thrown when error is encountered.</summary>
        public Exception? ErrorException { get; set; }
       
        /// <summary>Gets information about the success of the request.</summary>
        public bool IsSuccessful => (int) StatusCode >= 200 && (int) StatusCode <= 299;

        /// <summary>Creates the response containing an HTTP context.</summary>
        /// <param name="httpRequest">The HTTP request that the response belongs to.</param>
        /// <param name="httpResponseMessage">The System.Net.Http HTTP response message that the response belongs to.</param>
        public HttpResponse(
            IHttpRequest httpRequest, 
            HttpResponseMessage httpResponseMessage)
        {
            Request = httpRequest;
            
            _httpResponseMessage = httpResponseMessage;
        }

        internal HttpResponse(HttpResponse httpResponse)
        {
            Request = httpResponse.Request;
            ErrorMessage = httpResponse.ErrorMessage;
            ErrorException = httpResponse.ErrorException;
            
            _httpResponseMessage = httpResponse._httpResponseMessage;
        }

        /// <summary>Throws an exception if the IsSuccessful property for the HTTP response is false.</summary>
        public IHttpResponse EnsureSuccess()
        {
            if (!IsSuccessful)
                throw ErrorException!;
            return this;
        }
    }
}
