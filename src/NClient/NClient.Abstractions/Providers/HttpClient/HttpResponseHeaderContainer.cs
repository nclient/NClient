using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NClient.Abstractions.Providers.HttpClient
{
    public class HttpResponseHeaderContainer : IHttpResponseHeaderContainer
    {
        private readonly HttpResponseHeaders _httpResponseHeaders;

        public ICollection<string> AcceptRanges => _httpResponseHeaders.AcceptRanges;
        public TimeSpan? Age => _httpResponseHeaders.Age;
        public EntityTagHeaderValue ETag => _httpResponseHeaders.ETag;
        public Uri Location => _httpResponseHeaders.Location;
        public ICollection<AuthenticationHeaderValue> ProxyAuthenticate => _httpResponseHeaders.ProxyAuthenticate;
        public RetryConditionHeaderValue RetryAfter => _httpResponseHeaders.RetryAfter;
        public ICollection<ProductInfoHeaderValue> Server => _httpResponseHeaders.Server;
        public ICollection<string> Vary => _httpResponseHeaders.Vary;
        public ICollection<AuthenticationHeaderValue> WwwAuthenticate => _httpResponseHeaders.WwwAuthenticate;
        public CacheControlHeaderValue CacheControl => _httpResponseHeaders.CacheControl;
        public ICollection<string> Connection => _httpResponseHeaders.Connection;
        public bool? ConnectionClose => _httpResponseHeaders.ConnectionClose;
        public DateTimeOffset? Date => _httpResponseHeaders.Date;
        public ICollection<NameValueHeaderValue> Pragma => _httpResponseHeaders.Pragma;
        public ICollection<string> Trailer => _httpResponseHeaders.Trailer;
        public ICollection<TransferCodingHeaderValue> TransferEncoding => _httpResponseHeaders.TransferEncoding;
        public bool? TransferEncodingChunked => _httpResponseHeaders.TransferEncodingChunked;
        public ICollection<ProductHeaderValue> Upgrade => _httpResponseHeaders.Upgrade;
        public ICollection<ViaHeaderValue> Via => _httpResponseHeaders.Via;
        public ICollection<WarningHeaderValue> Warning => _httpResponseHeaders.Warning;
        
        public HttpResponseHeaderContainer(HttpResponseHeaders httpResponseHeaders)
        {
            _httpResponseHeaders = httpResponseHeaders;
        }

        public HttpResponseHeaderContainer(IEnumerable<IHttpHeader> httpHeaders)
        {
            var fakeHttpResponseMessage = new HttpResponseMessage();
            foreach (var headerValues in httpHeaders.GroupBy(x => x.Name))
            {
                fakeHttpResponseMessage.Headers.Add(headerValues.Key, headerValues.Select(x => x.Value));
            }
            
            _httpResponseHeaders = fakeHttpResponseMessage.Headers;
        }

        public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator() => _httpResponseHeaders.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
