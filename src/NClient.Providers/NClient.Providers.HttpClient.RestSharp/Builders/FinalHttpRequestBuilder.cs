using System;
using System.Linq;
using NClient.Abstractions.Providers.HttpClient;
using NClient.Abstractions.Providers.Serialization;
using NClient.Providers.HttpClient.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.HttpClient.RestSharp.Builders
{
    internal interface IFinalHttpRequestBuilder
    {
        IHttpRequest Build(IHttpRequest httpRequest, IRestRequest restRequest);
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
        
        public IHttpRequest Build(IHttpRequest httpRequest, IRestRequest restRequest)
        {
            var resource = new Uri(restRequest.Resource);
            var method = _restSharpMethodMapper.Map(restRequest.Method);
            var content = restRequest.Body is null ? null : _serializer.Serialize(restRequest.Body.Value);

            var finalRequest = new HttpRequest(httpRequest.Id, resource, method)
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
