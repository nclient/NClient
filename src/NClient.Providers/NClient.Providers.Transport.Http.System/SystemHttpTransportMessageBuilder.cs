using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using NClient.Providers.Resilience;
using NClient.Providers.Transport.Http.System.Builders;
using NClient.Providers.Transport.Http.System.Helpers;

// ReSharper disable UnusedVariable
namespace NClient.Providers.Transport.Http.System
{
    internal class SystemHttpTransportMessageBuilder : ITransportMessageBuilder<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISystemHttpMethodMapper _systemHttpMethodMapper;
        private readonly IFinalHttpRequestBuilder _finalHttpRequestBuilder;

        public SystemHttpTransportMessageBuilder(
            ISystemHttpMethodMapper systemHttpMethodMapper,
            IFinalHttpRequestBuilder finalHttpRequestBuilder)
        {
            _systemHttpMethodMapper = systemHttpMethodMapper;
            _finalHttpRequestBuilder = finalHttpRequestBuilder;
        }
        
        public Task<HttpRequestMessage> BuildTransportRequestAsync(IRequest request)
        {
            var parameters = request.Parameters
                .ToDictionary(x => x.Name, x => x.Value!.ToString());
            var uri = new Uri(QueryHelpers.AddQueryString(request.Endpoint, parameters));

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = _systemHttpMethodMapper.Map(request.Type), 
                RequestUri = uri
            };

            foreach (var metadata in request.Metadatas.SelectMany(x => x.Value))
            {
                httpRequestMessage.Headers.Add(metadata.Name, metadata.Value);
            }

            if (request.Content is not null)
            {
                httpRequestMessage.Content = new ByteArrayContent(request.Content.Bytes);
                
                foreach (var metadata in request.Content.Metadatas.SelectMany(x => x.Value))
                {
                    httpRequestMessage.Content.Headers.Add(metadata.Name, metadata.Value);
                }
            }

            return Task.FromResult(httpRequestMessage);
        }

        public async Task<IResponse> BuildResponseAsync(IRequest request, IResponseContext<HttpRequestMessage, HttpResponseMessage> responseContext)
        {
            var finalHttpRequest = await _finalHttpRequestBuilder
                .BuildAsync(request, responseContext.Response.RequestMessage)
                .ConfigureAwait(false);

            LoadLazyHeaders(responseContext.Response.Content?.Headers);
            
            var content = responseContext.Response.Content is null 
                ? new Content() 
                : new Content(
                    await responseContext.Response.Content.ReadAsByteArrayAsync().ConfigureAwait(false),
                    responseContext.Response.Content.Headers.ContentEncoding.FirstOrDefault(),
                    new MetadataContainer(responseContext.Response.Content.Headers.SelectMany(header => header.Value
                        .Select(value => new Metadata(header.Key, value)))));

            var httpResponse = new Response(finalHttpRequest)
            {
                Metadatas = new MetadataContainer(responseContext.Response.Headers
                    .SelectMany(header => header.Value
                        .Select(value => new Metadata(header.Key, value)))),
                Content = content,
                StatusCode = (int)responseContext.Response.StatusCode,
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

        private static void LoadLazyHeaders(HttpContentHeaders? httpContentHeaders = null)
        {
            if (httpContentHeaders is null)
                return;
            
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
            try
            {
                httpResponseMessage.EnsureSuccessStatusCode();
                return null;
            }
            catch (HttpRequestException e)
            {
                return e;
            }
        }
    }
}
