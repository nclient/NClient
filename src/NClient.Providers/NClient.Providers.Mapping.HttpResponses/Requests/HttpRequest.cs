using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Results.HttpResults
{
    /// <summary>The container for data used to make HTTP requests.</summary>
    public interface IHttpRequest
    {
        /// <summary>Gets the HTTP message version.</summary>
        Version Version { get; }
        
        /// <summary>Gets the contents of the HTTP request.</summary>
        HttpContent Content { get; }
        
        /// <summary>Gets the HTTP method used by the HTTP request.</summary>
        HttpMethod Method { get; }
        
        /// <summary>Gets the <see cref="T:System.Uri" /> used for the HTTP request.</summary>
        Uri RequestUri { get; }
        
        /// <summary>Gets the collection of HTTP request headers.</summary>
        HttpRequestHeaders Headers { get; }
        
        /// <summary>Gets a set of properties for the HTTP request.</summary>
        IDictionary<string, object> Properties { get; }
    }
    
    /// <summary>The container for data used to make HTTP requests.</summary>
    public class HttpRequest : IHttpRequest
    {
        private readonly HttpRequestMessage _httpRequestMessage;

        /// <summary>Gets the HTTP message version.</summary>
        public Version Version => _httpRequestMessage.Version;
        
        /// <summary>Gets the contents of the HTTP request.</summary>
        public HttpContent Content => _httpRequestMessage.Content;
        
        /// <summary>Gets the HTTP method used by the HTTP request.</summary>
        public HttpMethod Method => _httpRequestMessage.Method;
        
        /// <summary>Gets the <see cref="T:System.Uri" /> used for the HTTP request.</summary>
        public Uri RequestUri => _httpRequestMessage.RequestUri;
        
        /// <summary>Gets the collection of HTTP request headers.</summary>
        public HttpRequestHeaders Headers => _httpRequestMessage.Headers;
        
        /// <summary>Gets a set of properties for the HTTP request.</summary>
        public IDictionary<string, object> Properties => _httpRequestMessage.Properties;
        
        /// <summary>Initializes the container for data used to make HTTP requests.</summary>
        /// <param name="httpRequestMessage">The System.Net.Http container that represents a HTTP request message.</param>
        public HttpRequest(HttpRequestMessage httpRequestMessage)
        {
            _httpRequestMessage = httpRequestMessage;
        }
    }
}
