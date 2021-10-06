using System;
using System.Linq;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.HttpClient.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.HttpClient.RestSharp.Builders
{
    internal interface IFinalHttpRequestBuilder
    {
        IHttpRequest Build(IHttpRequest request, IRestRequest restRequest);
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
        
        public IHttpRequest Build(IHttpRequest request, IRestRequest restRequest)
        {
            var resource = new Uri(restRequest.Resource);
            var method = _restSharpMethodMapper.Map(restRequest.Method);
            var content = restRequest.Body is null ? null : _serializer.Serialize(restRequest.Body.Value);

            var finalRequest = new HttpRequest(request.Id, resource, method)
            {
                Body = restRequest.Body?.Value,
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
