using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using NClient.Abstractions.Providers.HttpClient;
using NClient.Abstractions.Providers.Serialization;

namespace NClient.Providers.Transport.Http.System.Builders
{
    internal interface IFinalHttpRequestBuilder
    {
        Task<IHttpRequest> BuildAsync(IHttpRequest httpRequest, HttpRequestMessage httpRequestMessage);
    }
    
    internal class FinalHttpRequestBuilder : IFinalHttpRequestBuilder
    {
        private readonly ISerializer _serializer;
        
        public FinalHttpRequestBuilder(ISerializer serializer)
        {
            _serializer = serializer;
        }
        
        public async Task<IHttpRequest> BuildAsync(IHttpRequest httpRequest, HttpRequestMessage httpRequestMessage)
        {
            var resource = new Uri(httpRequestMessage.RequestUri.GetLeftPart(UriPartial.Path));
            var content = httpRequestMessage.Content is null ? null : await httpRequestMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            var finalRequest = new HttpRequest(httpRequest.Id, resource, httpRequestMessage.Method)
            {
                Content = content,
                Data = content is not null && httpRequest.Data is not null 
                    ? _serializer.Deserialize(content, httpRequest.Data.GetType()) 
                    : null
            };

            var headers = httpRequestMessage.Headers?
                .Select(x => new HttpHeader(x.Key!, x.Value?.FirstOrDefault() ?? ""))
                .ToArray() ?? Array.Empty<IHttpHeader>();
            var contentHeaders = httpRequestMessage.Content?.Headers
                .Select(x => new HttpHeader(x.Key!, x.Value?.FirstOrDefault() ?? ""))
                .ToArray() ?? Array.Empty<IHttpHeader>();
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
