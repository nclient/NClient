using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Transport.SystemNetHttp.AspNetCore;
using NClient.Providers.Transport.SystemNetHttp.Helpers;

// ReSharper disable UnusedVariable
namespace NClient.Providers.Transport.SystemNetHttp
{
    internal class SystemNetHttpTransportRequestBuilder : ITransportRequestBuilder<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISystemNetHttpMethodMapper _systemNetHttpMethodMapper;

        public SystemNetHttpTransportRequestBuilder(ISystemNetHttpMethodMapper systemNetHttpMethodMapper)
        {
            _systemNetHttpMethodMapper = systemNetHttpMethodMapper;
        }
        
        public Task<HttpRequestMessage> BuildAsync(IRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var parameters = request.Parameters
                .Select(x => new KeyValuePair<string, string>(x.Name, x.Value!.ToString()))
                .ToArray();
            var uri = new Uri(QueryHelpers.AddQueryString(request.Resource.ToString(), parameters));

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = _systemNetHttpMethodMapper.Map(request.Type), 
                RequestUri = uri
            };

            foreach (var metadata in request.Metadatas.SelectMany(x => x.Value))
            {
                httpRequestMessage.Headers.Add(metadata.Name, metadata.Value);
            }

            if (request.Content is not null)
            {
                request.Content.Stream.Position = 0;

                #if NETSTANDARD2_0 || NETFRAMEWORK
                httpRequestMessage.Content = new LeavingOpenStreamContent(request.Content.Stream);
                #else
                httpRequestMessage.Content = new StreamContent(request.Content.Stream);
                #endif
                foreach (var metadata in request.Content.Metadatas.SelectMany(x => x.Value))
                {
                    httpRequestMessage.Content.Headers.Add(metadata.Name, metadata.Value);
                }
            }

            return Task.FromResult(httpRequestMessage);
        }
    }
}
