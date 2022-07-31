using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NClient.Models;
using NClient.Providers.Mapping;

namespace NClient.Providers.Transport.SystemNetHttp.Mapping
{
    public class HttpFileContentResponseMapper : IResponseMapper<HttpRequestMessage, HttpResponseMessage>
    {
        public bool CanMap(Type resultType, IResponseContext<HttpRequestMessage, HttpResponseMessage> responseContext)
        {
            return resultType == typeof(IFormFile) || resultType == typeof(HttpFileContent);
        }
        
        public async Task<object?> MapAsync(Type resultType, IResponseContext<HttpRequestMessage, HttpResponseMessage> responseContext, CancellationToken cancellationToken)
        {
            var stream = await responseContext.Response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            
            return new HttpFileContent(
                stream,
                responseContext.Response.Content.Headers.ContentDisposition?.Name!,
                responseContext.Response.Content.Headers.ContentDisposition?.FileName!,
                responseContext.Response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream",
                responseContext.Response.Content.Headers.ContentDisposition?.ToString() ?? "attachment",
                new HeaderDictionary(responseContext.Response.Content.Headers
                    .Select(x => 
                        new KeyValuePair<string, StringValues>(x.Key, new StringValues(x.Value.ToArray())))));
        }
    }
}
