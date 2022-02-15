using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

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

            var httpResponse = new Response(request)
            {
                Metadatas = new MetadataContainer(responseContext.Response.Headers
                    .SelectMany(header => header.Value
                        .Select(value => new Metadata(header.Key, value)))),
                Content = content,
                StatusCode = (int) responseContext.Response.StatusCode,
                StatusDescription = responseContext.Response.StatusCode.ToString(),
                Endpoint = responseContext.Response.RequestMessage.RequestUri.ToString(),
                ProtocolVersion = responseContext.Response.Version,
                IsSuccessful = responseContext.Response.IsSuccessStatusCode
            };
            
            var exception = TryGetException(responseContext.Response);
            httpResponse.ErrorMessage = exception?.Message;
            httpResponse.ErrorException = exception;
            
            return httpResponse;
        }

        private static async Task<Stream> GetStreamContentAsync(HttpResponseMessage transportResponse, bool allocateMemoryForContent)
        {
            Stream stream;
            if (allocateMemoryForContent)
            {
                var bytes = await transportResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                stream = new MemoryStream(bytes);
            }
            else
            {
                stream = await transportResponse.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }

            LoadLazyHeaders(transportResponse.Content.Headers);
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

        private static HttpRequestException? TryGetException(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
                return null;
            
            #if NET5_0_OR_GREATER
            return new HttpRequestException(
                GetErrorMessage(httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase), 
                inner: null, 
                httpResponseMessage.StatusCode);
            #else
            return new HttpRequestException(
                GetErrorMessage(httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase), 
                inner: null);
            #endif
        }
        
        private static string GetErrorMessage(HttpStatusCode httpStatusCode, string reasonPhrase)
        {
            return string.Format(
                CultureInfo.InvariantCulture, 
                format: "Response status code does not indicate success: {0} ({1}).", 
                httpStatusCode, reasonPhrase);
        }
    }
}
