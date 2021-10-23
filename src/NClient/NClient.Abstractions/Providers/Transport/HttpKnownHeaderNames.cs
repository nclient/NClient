﻿using System.Collections;
using System.Collections.Generic;

namespace NClient.Providers.Transport
{
    public static class HttpKnownHeaderNames
    {
        public const string CacheControl = "Cache-Control";
        public const string Connection = "Connection";
        public const string Date = "Date";
        public const string KeepAlive = "Keep-Alive";
        public const string Pragma = "Pragma";
        public const string ProxyConnection = "Proxy-Connection";
        public const string Trailer = "Trailer";
        public const string TransferEncoding = "Transfer-Encoding";
        public const string Upgrade = "Upgrade";
        public const string Via = "Via";
        public const string Warning = "Warning";
        public const string ContentLength = "Content-Length";
        public const string ContentType = "Content-Type";
        public const string ContentDisposition = "Content-Disposition";
        public const string ContentEncoding = "Content-Encoding";
        public const string ContentLanguage = "Content-Language";
        public const string ContentLocation = "Content-Location";
        public const string ContentRange = "Content-Range";
        public const string Expires = "Expires";
        public const string LastModified = "Last-Modified";
        public const string Age = "Age";
        public const string Location = "Location";
        public const string ProxyAuthenticate = "Proxy-Authenticate";
        public const string RetryAfter = "Retry-After";
        public const string Server = "Server";
        public const string SetCookie = "Set-Cookie";
        public const string SetCookie2 = "Set-Cookie2";
        public const string Vary = "Vary";
        public const string WWWAuthenticate = "WWW-Authenticate";
        public const string Accept = "Accept";
        public const string AcceptCharset = "Accept-Charset";
        public const string AcceptEncoding = "Accept-Encoding";
        public const string AcceptLanguage = "Accept-Language";
        public const string Authorization = "Authorization";
        public const string Cookie = "Cookie";
        public const string Cookie2 = "Cookie2";
        public const string Expect = "Expect";
        public const string From = "From";
        public const string Host = "Host";
        public const string IfMatch = "If-Match";
        public const string IfModifiedSince = "If-Modified-Since";
        public const string IfNoneMatch = "If-None-Match";
        public const string IfRange = "If-Range";
        public const string IfUnmodifiedSince = "If-Unmodified-Since";
        public const string MaxForwards = "Max-Forwards";
        public const string ProxyAuthorization = "Proxy-Authorization";
        public const string Referer = "Referer";
        public const string Range = "Range";
        public const string UserAgent = "User-Agent";
        public const string ContentMD5 = "Content-MD5";
        public const string ETag = "ETag";
        public const string TE = "TE";
        public const string Allow = "Allow";
        public const string AcceptRanges = "Accept-Ranges";
        public const string P3P = "P3P";
        public const string XPoweredBy = "X-Powered-By";
        public const string XAspNetVersion = "X-AspNet-Version";
        public const string SecWebSocketKey = "Sec-WebSocket-Key";
        public const string SecWebSocketExtensions = "Sec-WebSocket-Extensions";
        public const string SecWebSocketAccept = "Sec-WebSocket-Accept";
        public const string Origin = "Origin";
        public const string SecWebSocketProtocol = "Sec-WebSocket-Protocol";
        public const string SecWebSocketVersion = "Sec-WebSocket-Version"; 
    }
    
    public class HttpContentKnownHeaderNames : IEnumerable<string>
    {
        private static readonly string[] Names = new[]
        {
            ContentLength,
            ContentType,
            ContentDisposition,
            ContentEncoding,
            ContentLanguage,
            ContentLocation,
            ContentRange,
            ContentRange,
            Expires,
            LastModified
        };
        
        public const string ContentLength = HttpKnownHeaderNames.ContentLength;
        public const string ContentType = HttpKnownHeaderNames.ContentType;
        public const string ContentDisposition = HttpKnownHeaderNames.ContentDisposition;
        public const string ContentEncoding = HttpKnownHeaderNames.ContentEncoding;
        public const string ContentLanguage = HttpKnownHeaderNames.ContentLanguage;
        public const string ContentLocation = HttpKnownHeaderNames.ContentLocation;
        public const string ContentRange = HttpKnownHeaderNames.ContentRange;
        public const string Expires = HttpKnownHeaderNames.Expires;
        public const string LastModified = HttpKnownHeaderNames.LastModified;
        
        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)Names).GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
