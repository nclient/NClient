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
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;

        public FinalHttpRequestBuilder(IRestSharpMethodMapper restSharpMethodMapper)
        {
            _restSharpMethodMapper = restSharpMethodMapper;
        }
        
        public IRequest Build(IRequest request, IRestRequest restRequest)
        {
            var method = _restSharpMethodMapper.Map(restRequest.Method);

            var finalRequest = new Request(request.Id, restRequest.Resource, method)
            {
                // TODO: should use content from restRequest
                Content = request.Content
            };

            // TODO: filter content headers
            foreach (var header in restRequest.Parameters.Where(x => x.Type == ParameterType.HttpHeader))
            {
                finalRequest.AddMetadata(header.Name!, header.Value?.ToString() ?? "");
            }
            
            foreach (var parameter in restRequest.Parameters.Where(x => x.Type == ParameterType.QueryString))
            {
                finalRequest.AddParameter(parameter.Name!, parameter.Value?.ToString() ?? "");
            }

            return finalRequest;
        }
    }
}
