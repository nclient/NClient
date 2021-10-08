using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Mapping;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.HttpClient.System.Mapping
{
    public class CommonResponseMapper : IResponseMapper<HttpResponseMessage>
    {
        public bool CanMapTo(Type resultType)
        {
            return true;
        }
        
        public async Task<object?> MapAsync(Type resultType, IHttpRequest httpRequest, HttpResponseMessage response, ISerializer serializer)
        {
            var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var json = AsString(bytes, response.Content.Headers.ContentEncoding.FirstOrDefault());
            return serializer.Deserialize(json, resultType);
        }
        
        private static string AsString(byte[]? bytes, string? contentEncoding)
        {
            if (string.IsNullOrEmpty(contentEncoding))
                return AsString(bytes, Encoding.UTF8);

            try
            {
                var encoding = Encoding.GetEncoding(contentEncoding);
                return AsString(bytes, encoding);
            }
            catch (ArgumentException)
            {
                return AsString(bytes, Encoding.UTF8);
            }
        }

        private static string AsString(byte[]? bytes, Encoding encoding)
        {
            return bytes == null ? "" : encoding.GetString(bytes, 0, bytes.Length);
        }
    }
}
