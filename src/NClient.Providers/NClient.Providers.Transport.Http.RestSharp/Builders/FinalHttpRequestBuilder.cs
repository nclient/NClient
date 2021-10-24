using System;
using System.Linq;
using NClient.Providers.Serialization;
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
        private readonly ISerializer _serializer;
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;

        public FinalHttpRequestBuilder(
            ISerializer serializer,
            IRestSharpMethodMapper restSharpMethodMapper)
        {
            _serializer = serializer;
            _restSharpMethodMapper = restSharpMethodMapper;
        }
        
        public IRequest Build(IRequest request, IRestRequest restRequest)
        {
            var resource = new Uri(restRequest.Resource);
            var method = _restSharpMethodMapper.Map(restRequest.Method);
            var content = restRequest.Body is null ? null : _serializer.Serialize(restRequest.Body.Value);

            var finalRequest = new Request(request.Id, resource, method)
            {
                Data = restRequest.Body?.Value,
                Content = content
            };

            foreach (var header in restRequest.Parameters.Where(x => x.Type == ParameterType.HttpHeader))
            {
                finalRequest.AddHeader(header.Name!, header.Value?.ToString() ?? "");
            }
            
            foreach (var parameter in restRequest.Parameters.Where(x => x.Type == ParameterType.QueryString))
            {
                finalRequest.AddParameter(parameter.Name!, parameter.Value?.ToString() ?? "");
            }

            return finalRequest;
        }
    }
}
