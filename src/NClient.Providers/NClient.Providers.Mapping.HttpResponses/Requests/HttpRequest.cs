using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Results.HttpResults
{
    public interface IHttpRequest
    {
        Version Version { get; }
        HttpContent Content { get; }
        HttpMethod Method { get; }
        Uri RequestUri { get; }
        HttpRequestHeaders Headers { get; }
        IDictionary<string, object> Properties { get; }
    }
    
    /// <summary>
    /// The container for data used to make requests.
    /// </summary>
    public class HttpRequest : IHttpRequest
    {
        private readonly HttpRequestMessage _httpRequestMessage;

        public Version Version => _httpRequestMessage.Version;
        public HttpContent Content => _httpRequestMessage.Content;
        public HttpMethod Method => _httpRequestMessage.Method;
        public Uri RequestUri => _httpRequestMessage.RequestUri;
        public HttpRequestHeaders Headers => _httpRequestMessage.Headers;
        public IDictionary<string, object> Properties => _httpRequestMessage.Properties;
        
        public HttpRequest(HttpRequestMessage httpRequestMessage)
        {
            _httpRequestMessage = httpRequestMessage;
        }
    }
}
