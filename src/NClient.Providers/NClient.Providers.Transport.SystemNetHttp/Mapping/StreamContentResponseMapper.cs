using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NClient.Models;
using NClient.Providers.Mapping;
using StreamContent = NClient.Models.StreamContent;

namespace NClient.Providers.Transport.SystemNetHttp.Mapping
{
    public class StreamContentResponseMapper : IResponseMapper<HttpRequestMessage, HttpResponseMessage>
    {
        public bool CanMap(Type resultType, IResponseContext<HttpRequestMessage, HttpResponseMessage> responseContext)
        {
            return resultType == typeof(IStreamContent) || resultType == typeof(StreamContent);
        }
        
        public async Task<object?> MapAsync(Type resultType, IResponseContext<HttpRequestMessage, HttpResponseMessage> responseContext, CancellationToken cancellationToken)
        {
            var stream = await responseContext.Response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var encodingName = responseContext.Response.Content.Headers.ContentEncoding.FirstOrDefault();
            var encoding = string.IsNullOrEmpty(encodingName) ? Encoding.UTF8 : Encoding.GetEncoding(encodingName);
            var metadatas = responseContext.Response.Content.Headers.SelectMany(header => header.Value
                .Select(value => new Metadata(header.Key, value)));
            
            return new StreamContent(
                stream,
                encoding,
                responseContext.Response.Content.Headers.ContentType.MediaType,
                responseContext.Response.Content.Headers.ContentDisposition.Name,
                metadatas);
        }
    }
}
