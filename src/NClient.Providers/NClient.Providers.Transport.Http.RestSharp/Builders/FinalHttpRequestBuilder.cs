using System;
using System.Collections.Generic;
using System.Linq;
using NClient.Providers.Transport.Http.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.Transport.Http.RestSharp.Builders
{
    internal interface IFinalHttpRequestBuilder
    {
        IRequest Build(IRequest request, IRestRequest restRequest);
    }
    
    internal class FinalHttpRequestBuilder : IFinalHttpRequestBuilder
    {
        private static readonly HashSet<string> ContentHeaderNames = new(new HttpContentKnownHeaderNames());
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;

        public FinalHttpRequestBuilder(IRestSharpMethodMapper restSharpMethodMapper)
        {
            _restSharpMethodMapper = restSharpMethodMapper;
        }
        
        public IRequest Build(IRequest request, IRestRequest restRequest)
        {
            var method = _restSharpMethodMapper.Map(restRequest.Method);
            
            var allHeaders = restRequest.Parameters
                .Where(x => x.Type == ParameterType.HttpHeader)
                .ToArray();
            var requestHeaders = allHeaders
                .Where(x => x.Name is not null && !ContentHeaderNames.Contains(x.Name))
                .Select(x => new Metadata(x.Name!, x.Value?.ToString() ?? ""))
                .ToArray();
            var contentHeaders = allHeaders
                .Where(x => x.Name is not null && ContentHeaderNames.Contains(x.Name))
                .Select(x => new Metadata(x.Name!, x.Value?.ToString() ?? ""))
                .ToArray();

            var finalRequest = new Request(request.Id, restRequest.Resource, method)
            {
                Content = new Content(
                    request.Content?.Bytes.ToArray(), 
                    encoding: request.Content?.Encoding?.WebName,
                    headerContainer: new MetadataContainer(contentHeaders)),
                Timeout = TimeSpan.FromMilliseconds(restRequest.Timeout)
            };
            
            foreach (var header in requestHeaders)
            {
                finalRequest.AddMetadata(header.Name, header.Value);
            }
            
            foreach (var parameter in restRequest.Parameters.Where(x => x.Type == ParameterType.QueryString))
            {
                finalRequest.AddParameter(parameter.Name!, parameter.Value?.ToString() ?? "");
            }

            return finalRequest;
        }
    }
}
