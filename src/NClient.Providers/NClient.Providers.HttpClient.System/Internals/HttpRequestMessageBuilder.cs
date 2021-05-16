using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using NClient.Abstractions.HttpClients;

namespace NClient.Providers.HttpClient.System.Internals
{
    public class HttpRequestMessageBuilder
    {
        public HttpRequestMessage Build(HttpRequest request)
        {
            var parameters = request.Parameters
                .ToDictionary(x => x.Name, x => x.Value!.ToString());
            var uri = new Uri(QueryHelpers.AddQueryString(request.Uri.ToString(), parameters));

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = request.Method,
                RequestUri = uri
            };

            httpRequestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            if (request.Body != null)
            {
                var body = JsonSerializer.Serialize(request.Body);
                httpRequestMessage.Content = new StringContent(body, Encoding.UTF8, mediaType: "application/json");
            }

            foreach (var header in request.Headers)
            {
                httpRequestMessage.Headers.Add(header.Name, header.Value);
            }

            return httpRequestMessage;
        }
    }
}