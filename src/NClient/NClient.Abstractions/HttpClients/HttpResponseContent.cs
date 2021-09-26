using System;
using System.Linq;
using System.Text;

namespace NClient.Abstractions.HttpClients
{
    public class HttpResponseContent
    {
        public byte[] Bytes { get; }
        public HttpResponseContentHeaderContainer Headers { get; }

        public HttpResponseContent(byte[]? bytes = null, HttpResponseContentHeaderContainer? headerContainer = null)
        {
            Bytes = bytes ?? Array.Empty<byte>();
            Headers = headerContainer ?? new HttpResponseContentHeaderContainer(Array.Empty<HttpHeader>());
        }

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
