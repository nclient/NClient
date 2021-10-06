using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NClient.Abstractions.HttpClients
{
    public class HttpResponseContentHeaderContainer : IHttpResponseContentHeaderContainer
    {
        private readonly HttpContentHeaders _httpContentHeaders;

        public ICollection<string> Allow => _httpContentHeaders.Allow;
        public ContentDispositionHeaderValue ContentDisposition => _httpContentHeaders.ContentDisposition;
        public ICollection<string> ContentEncoding => _httpContentHeaders.ContentEncoding;
        public ICollection<string> ContentLanguage => _httpContentHeaders.ContentLanguage;
        public long? ContentLength => _httpContentHeaders.ContentLength;
        public Uri ContentLocation => _httpContentHeaders.ContentLocation;
        // ReSharper disable once InconsistentNaming
        public byte[] ContentMD5 => _httpContentHeaders.ContentMD5;
        public ContentRangeHeaderValue ContentRange => _httpContentHeaders.ContentRange;
        public MediaTypeHeaderValue ContentType => _httpContentHeaders.ContentType;
        public DateTimeOffset? Expires => _httpContentHeaders.Expires;
        public DateTimeOffset? LastModified => _httpContentHeaders.LastModified;
        
        public HttpResponseContentHeaderContainer(HttpContentHeaders httpContentHeaders)
        {
            _httpContentHeaders = httpContentHeaders;
        }
        
        public HttpResponseContentHeaderContainer(IEnumerable<HttpHeader> httpHeaders)
        {
            var fakeHttpContent = new ByteArrayContent(Array.Empty<byte>());
            foreach (var headerValues in httpHeaders.GroupBy(x => x.Name))
            {
                fakeHttpContent.Headers.Add(headerValues.Key, headerValues.Select(x => x.Value));
            }
            
            _httpContentHeaders = fakeHttpContent.Headers;
        }
        
        public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator() => _httpContentHeaders.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
