﻿using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using NClient.Providers.Transport.SystemNetHttp.Helpers;

namespace NClient.Providers.Transport.SystemNetHttp.Builders
{
    internal interface IFinalHttpRequestBuilder
    {
        Task<IRequest> BuildAsync(IRequest request, HttpRequestMessage httpRequestMessage);
    }
    
    internal class FinalHttpRequestBuilder : IFinalHttpRequestBuilder
    {
        private readonly ISystemNetHttpMethodMapper _systemNetHttpMethodMapper;

        public FinalHttpRequestBuilder(ISystemNetHttpMethodMapper systemNetHttpMethodMapper)
        {
            _systemNetHttpMethodMapper = systemNetHttpMethodMapper;
        }
        
        public async Task<IRequest> BuildAsync(IRequest request, HttpRequestMessage httpRequestMessage)
        {
            var endpoint = new Uri(httpRequestMessage.RequestUri.GetLeftPart(UriPartial.Path));
            var method = _systemNetHttpMethodMapper.Map(httpRequestMessage.Method);
            
            // Workaround for the framework:
            var disposedFiled = typeof(HttpContent).GetField(name: "disposed", BindingFlags.NonPublic | BindingFlags.Instance);
            var contentIsDisposed = httpRequestMessage.Content is not null
                ? disposedFiled?.GetValue(httpRequestMessage.Content)
                : null;

            var contentBytes = contentIsDisposed as bool? == false
                ? httpRequestMessage.Content is null ? null : await httpRequestMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false)
                : request.Content?.Bytes;
            var contentEncoding = httpRequestMessage.Content?.Headers.ContentEncoding.FirstOrDefault();
            var contentHeaders = httpRequestMessage.Content?.Headers
                .Select(x => new Metadata(x.Key!, x.Value?.FirstOrDefault() ?? ""))
                .ToArray() ?? Array.Empty<IMetadata>();
            
            var finalRequest = new Request(request.Id, endpoint.ToString(), method)
            {
                Content = contentBytes is null
                    ? null
                    : new Content(contentBytes, contentEncoding, new MetadataContainer(contentHeaders)),
                Timeout = httpRequestMessage.GetTimeout()
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
