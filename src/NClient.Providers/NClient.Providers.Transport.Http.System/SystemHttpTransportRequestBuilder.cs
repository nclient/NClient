﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using NClient.Providers.Transport.Http.System.Helpers;

// ReSharper disable UnusedVariable
namespace NClient.Providers.Transport.Http.System
{
    internal class SystemHttpTransportRequestBuilder : ITransportRequestBuilder<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISystemHttpMethodMapper _systemHttpMethodMapper;

        public SystemHttpTransportRequestBuilder(ISystemHttpMethodMapper systemHttpMethodMapper)
        {
            _systemHttpMethodMapper = systemHttpMethodMapper;
        }
        
        public Task<HttpRequestMessage> BuildAsync(IRequest request)
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
    }
}
