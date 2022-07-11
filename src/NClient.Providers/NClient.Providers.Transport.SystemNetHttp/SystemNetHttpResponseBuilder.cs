using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Transport.SystemNetHttp.Helpers;

// ReSharper disable UnusedVariable
namespace NClient.Providers.Transport.SystemNetHttp
{
    internal class SystemNetHttpResponseBuilder : IResponseBuilder<HttpRequestMessage, HttpResponseMessage>
    {
        public async Task<IResponse> BuildAsync(IRequest request, 
            IResponseContext<HttpRequestMessage, HttpResponseMessage> responseContext, bool allocateMemoryForContent,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var content = responseContext.Response.Content is null 
                ? new Content() 
                : new Content(
                    streamContent: await GetStreamContentAsync(responseContext.Response, allocateMemoryForContent).ConfigureAwait(false),
                    encoding: responseContext.Response.Content.Headers.ContentEncoding.FirstOrDefault(),
                    headerContainer: new MetadataContainer(responseContext.Response.Content.Headers.SelectMany(header => header.Value
                        .Select(value => new Metadata(header.Key, value)))));

            var response = new Response(request, disposeWith: new IDisposable[] { responseContext.Request, responseContext.Response })
            {
                Metadatas = new MetadataContainer(responseContext.Response.Headers
                    .SelectMany(header => header.Value
                        .Select(value => new Metadata(header.Key, value)))),
                Content = content,
                StatusCode = (int) responseContext.Response.StatusCode,
                StatusDescription = responseContext.Response.StatusCode.ToString(),
                Resource = responseContext.Response.RequestMessage.RequestUri,
                ProtocolVersion = responseContext.Response.Version,
                IsSuccessful = responseContext.Response.IsSuccessStatusCode
            };
            
            var exception = responseContext.Response.TryGetException();
            response.ErrorMessage = exception?.Message;
            response.ErrorException = exception;
            
            return response;
        }

        private static async Task<Stream> GetStreamContentAsync(HttpResponseMessage httpResponseMessage, bool allocateMemoryForContent)
        {
            Stream stream;
            if (allocateMemoryForContent)
            {
                var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                stream = new MemoryStream(bytes);
            }
            else
            {
                stream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }

            LoadLazyHeaders(httpResponseMessage.Content.Headers);
            return stream;
        }
        
        private static void LoadLazyHeaders(HttpContentHeaders httpContentHeaders)
        {
            // NOTE: HttpResponseContent supports lazy initialization of headers, but the content has already been received here, which means that lazy initialization is not needed.
            // When calling the ContentLength property, lazy initialization is triggered, but this is not documented. Perhaps this also works for other headers, so all properties are called.
            var unusedAllow = httpContentHeaders.Allow;
            var unusedContentLength = httpContentHeaders.ContentLength;
            var unusedContentDisposition = httpContentHeaders.ContentDisposition;
            var unusedContentEncoding = httpContentHeaders.ContentEncoding;
            var unusedContentLanguage = httpContentHeaders.ContentLanguage;
            var unusedContentLocation = httpContentHeaders.ContentLocation;
            var unusedContentRange = httpContentHeaders.ContentRange;
            var unusedContentType = httpContentHeaders.ContentType;
            var unusedLastModified = httpContentHeaders.LastModified;
            var unusedContentMd5 = httpContentHeaders.ContentMD5;
            var unusedExpires = httpContentHeaders.Expires;
        }
    }
}
