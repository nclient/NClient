using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.HttpClient.System.Builders
{
    internal interface IFinalHttpRequestBuilder
    {
        Task<HttpRequest> BuildAsync(HttpRequest request, HttpRequestMessage httpRequestMessage);
    }
    
    internal class FinalHttpRequestBuilder : IFinalHttpRequestBuilder
    {
        private readonly ISerializer _serializer;
        
        public FinalHttpRequestBuilder(ISerializer serializer)
        {
            _serializer = serializer;
        }
        
        public async Task<HttpRequest> BuildAsync(HttpRequest request, HttpRequestMessage httpRequestMessage)
        {
            var resource = new Uri(httpRequestMessage.RequestUri.GetLeftPart(UriPartial.Path));
            var content = httpRequestMessage.Content is null 
                ? null 
                : await httpRequestMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            var finalRequest = new HttpRequest(request.Id, resource, httpRequestMessage.Method)
            {
                Body = content is not null && request.Body is not null 
                    ? _serializer.Deserialize(content, request.Body.GetType()) 
                    : null,
                Content = content
            };

            foreach (var header in httpRequestMessage.Headers)
            {
                finalRequest.AddHeader(header.Key, header.Value.FirstOrDefault() ?? "");
            }

            var queryParameterCollection = HttpUtility.ParseQueryString(httpRequestMessage.RequestUri.Query);
            foreach (var parameterName in queryParameterCollection.AllKeys)
            {
                var parameterValue = queryParameterCollection[parameterName];
                finalRequest.AddParameter(parameterName, parameterValue);
            }

            return finalRequest;
        }
    }
}
