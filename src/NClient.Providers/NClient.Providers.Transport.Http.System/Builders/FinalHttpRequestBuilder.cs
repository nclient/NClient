using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using NClient.Providers.Transport.Http.System.Helpers;

namespace NClient.Providers.Transport.Http.System.Builders
{
    internal interface IFinalHttpRequestBuilder
    {
        Task<IRequest> BuildAsync(IRequest request, HttpRequestMessage httpRequestMessage);
    }
    
    internal class FinalHttpRequestBuilder : IFinalHttpRequestBuilder
    {
        private readonly ISystemHttpMethodMapper _systemHttpMethodMapper;

        public FinalHttpRequestBuilder(ISystemHttpMethodMapper systemHttpMethodMapper)
        {
            _systemHttpMethodMapper = systemHttpMethodMapper;
        }
        
        public async Task<IRequest> BuildAsync(IRequest request, HttpRequestMessage httpRequestMessage)
        {
            var resource = new Uri(httpRequestMessage.RequestUri.GetLeftPart(UriPartial.Path));
            var method = _systemHttpMethodMapper.Map(httpRequestMessage.Method);
            
            var contentBytes = httpRequestMessage.Content is null ? null : await httpRequestMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var contentEncoding = httpRequestMessage.Content?.Headers.ContentEncoding.FirstOrDefault();
            var contentHeaders = httpRequestMessage.Content?.Headers
                .Select(x => new Metadata(x.Key!, x.Value?.FirstOrDefault() ?? ""))
                .ToArray() ?? Array.Empty<IMetadata>();
            
            var finalRequest = new Request(request.Id, resource.ToString(), method)
            {
                Content = contentBytes is null
                    ? null
                    : new Content(contentBytes, contentEncoding, new MetadataContainer(contentHeaders))
            };

            var headers = httpRequestMessage.Headers?
                .Select(x => new Metadata(x.Key!, x.Value?.FirstOrDefault() ?? ""))
                .ToArray() ?? Array.Empty<IMetadata>();
            foreach (var header in headers)
            {
                finalRequest.AddMetadata(header.Name, header.Value);
            }

            var queryParameterCollection = HttpUtility.ParseQueryString(httpRequestMessage.RequestUri.Query);
            foreach (var parameterName in queryParameterCollection.AllKeys)
            {
                var parameterValue = queryParameterCollection[parameterName] ?? "";
                finalRequest.AddParameter(parameterName, parameterValue);
            }

            return finalRequest;
        }
    }
}
