using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.HttpClient.System
{
    internal class SystemHttpMessageBuilder : IHttpMessageBuilder<HttpRequestMessage>
    {
        private readonly ISerializer _serializer;

        public SystemHttpMessageBuilder(ISerializer serializer)
        {
            _serializer = serializer;
        }
        
        public Task<HttpRequestMessage> BuildRequestAsync(IHttpRequest httpRequest)
        {
            var parameters = httpRequest.Parameters
                .ToDictionary(x => x.Name, x => x.Value!.ToString());
            var uri = new Uri(QueryHelpers.AddQueryString(httpRequest.Resource.ToString(), parameters));

            var httpRequestMessage = new HttpRequestMessage { Method = httpRequest.Method, RequestUri = uri };

            httpRequestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(_serializer.ContentType));

            foreach (var header in httpRequest.Headers)
            {
                httpRequestMessage.Headers.Add(header.Name, header.Value);
            }
            
            if (httpRequest.Data != null)
            {
                var body = _serializer.Serialize(httpRequest.Data);
                httpRequestMessage.Content = new StringContent(body, Encoding.UTF8, _serializer.ContentType);
            }

            return Task.FromResult(httpRequestMessage);
        }
    }
}
