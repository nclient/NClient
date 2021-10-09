﻿using System;
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
        Task<IHttpRequest> BuildAsync(Guid requestId, Type? dataType, HttpRequestMessage httpRequestMessage);
    }
    
    internal class FinalHttpRequestBuilder : IFinalHttpRequestBuilder
    {
        private readonly ISerializer _serializer;
        
        public FinalHttpRequestBuilder(ISerializer serializer)
        {
            _serializer = serializer;
        }
        
        public async Task<IHttpRequest> BuildAsync(Guid requestId, Type? dataType, HttpRequestMessage httpRequestMessage)
        {
            var resource = new Uri(httpRequestMessage.RequestUri.GetLeftPart(UriPartial.Path));
            var content = httpRequestMessage.Content is null ? null : await httpRequestMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            var finalRequest = new HttpRequest(requestId, resource, httpRequestMessage.Method)
            {
                Content = content,
                Data = content is not null && dataType is not null 
                    ? _serializer.Deserialize(content, dataType) 
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