using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using NClient.Providers.Serialization;
using NClient.Providers.Transport.Http.System.Helpers;

namespace NClient.Providers.Transport.Http.System.Builders
{
    internal interface IFinalHttpRequestBuilder
    {
        Task<IRequest> BuildAsync(IRequest transportRequest, HttpRequestMessage httpRequestMessage);
    }
    
    internal class FinalHttpRequestBuilder : IFinalHttpRequestBuilder
    {
        private readonly ISerializer _serializer;
        private readonly ISystemHttpMethodMapper _systemHttpMethodMapper;

        public FinalHttpRequestBuilder(
            ISerializer serializer,
            ISystemHttpMethodMapper systemHttpMethodMapper)
        {
            _serializer = serializer;
            _systemHttpMethodMapper = systemHttpMethodMapper;
        }
        
        public async Task<IRequest> BuildAsync(IRequest transportRequest, HttpRequestMessage httpRequestMessage)
        {
            var resource = new Uri(httpRequestMessage.RequestUri.GetLeftPart(UriPartial.Path));
            var method = _systemHttpMethodMapper.Map(httpRequestMessage.Method);
            var content = httpRequestMessage.Content is null ? null : await httpRequestMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            var finalRequest = new Request(transportRequest.Id, resource, method)
            {
                Content = content,
                Data = content is not null && transportRequest.Data is not null 
                    ? _serializer.Deserialize(content, transportRequest.Data.GetType()) 
                    : null
            };

            var headers = httpRequestMessage.Headers?
                .Select(x => new Header(x.Key!, x.Value?.FirstOrDefault() ?? ""))
                .ToArray() ?? Array.Empty<IHeader>();
            var contentHeaders = httpRequestMessage.Content?.Headers
                .Select(x => new Header(x.Key!, x.Value?.FirstOrDefault() ?? ""))
                .ToArray() ?? Array.Empty<IHeader>();
            foreach (var header in headers.Concat(contentHeaders))
            {
                finalRequest.AddHeader(header.Name, header.Value);
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
