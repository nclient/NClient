using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace NClient.Abstractions.Providers.Transport
{
    /// <summary>
    /// Response content.
    /// </summary>
    public class HttpResponseContent : IHttpResponseContent
    {
        /// <summary>
        /// Gets byte representation of response content.
        /// </summary>
        public byte[] Bytes { get; }
        /// <summary>
        /// Gets headers returned by server with the response content.
        /// </summary>
        public IHttpResponseContentHeaderContainer Headers { get; }

        [SuppressMessage("ReSharper", "UnusedVariable")]
        public HttpResponseContent(byte[]? bytes = null, IHttpResponseContentHeaderContainer? headerContainer = null)
        {
            Bytes = bytes ?? Array.Empty<byte>();
            Headers = headerContainer ?? new HttpResponseContentHeaderContainer(Array.Empty<IHttpHeader>());
            
            // NOTE: HttpResponseContent supports lazy initialization of headers, but the content has already been received here, which means that lazy initialization is not needed.
            // When calling the ContentLength property, lazy initialization is triggered, but this is not documented. Perhaps this also works for other headers, so all properties are called.
            var unusedAllow = Headers.Allow;
            var unusedContentLength = Headers.ContentLength;
            var unusedContentDisposition = Headers.ContentDisposition;
            var unusedContentEncoding = Headers.ContentEncoding;
            var unusedContentLanguage = Headers.ContentLanguage;
            var unusedContentLocation = Headers.ContentLocation;
            var unusedContentRange = Headers.ContentRange;
            var unusedContentType = Headers.ContentType;
            var unusedLastModified = Headers.LastModified;
            var unusedContentMd5 = Headers.ContentMD5;
            var unusedExpires = Headers.Expires;
        }

        /// <summary>
        /// Gets string representation of response content.
        /// </summary>
        public override string ToString()
        {
            if (Headers.ContentEncoding.Count == 0 || string.IsNullOrEmpty(Headers.ContentEncoding.First()))
                return AsString(Bytes, Encoding.UTF8);

            try
            {
                var encoding = Encoding.GetEncoding(Headers.ContentEncoding.First());
                return AsString(Bytes, encoding);
            }
            catch (ArgumentException)
            {
                return AsString(Bytes, Encoding.UTF8);
            }
        }

        private static string AsString(byte[]? buffer, Encoding encoding)
        {
            return buffer == null ? "" : encoding.GetString(buffer, 0, buffer.Length);
        }
    }
}
