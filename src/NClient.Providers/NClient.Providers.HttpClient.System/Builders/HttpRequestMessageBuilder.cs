using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.HttpClient.System.Builders
{
    internal interface IHttpRequestMessageBuilder
    {
        HttpRequestMessage Build(HttpRequest request);
    }

    internal class HttpRequestMessageBuilder : IHttpRequestMessageBuilder
    {
        private readonly ISerializer _serializer;

        public HttpRequestMessageBuilder(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public HttpRequestMessage Build(HttpRequest request)
        {
            var parameters = request.Parameters
                .ToDictionary(x => x.Name, x => x.Value!.ToString());
            var uri = new Uri(QueryHelpers.AddQueryString(request.Resource.ToString(), parameters));

            var httpRequestMessage = new HttpRequestMessage { Method = request.Method, RequestUri = uri };

            httpRequestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(_serializer.ContentType));

            if (request.Body != null)
            {
                var body = _serializer.Serialize(request.Body);
                httpRequestMessage.Content = new StringContent(body, Encoding.UTF8, _serializer.ContentType);
            }

            foreach (var header in request.Headers)
            {
                httpRequestMessage.Headers.Add(header.Name, header.Value);
            }

            return httpRequestMessage;
        }
    }
}
