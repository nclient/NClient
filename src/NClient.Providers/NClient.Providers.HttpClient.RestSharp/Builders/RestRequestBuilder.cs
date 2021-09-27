using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.HttpClient.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.HttpClient.RestSharp.Builders
{
    internal interface IRestRequestBuilder
    {
        IRestRequest Build(HttpRequest request);
    }

    internal class RestRequestBuilder : IRestRequestBuilder
    {
        private readonly ISerializer _serializer;
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;

        public RestRequestBuilder(
            ISerializer serializer,
            IRestSharpMethodMapper restSharpMethodMapper)
        {
            _serializer = serializer;
            _restSharpMethodMapper = restSharpMethodMapper;
        }

        public IRestRequest Build(HttpRequest request)
        {
            var method = _restSharpMethodMapper.Map(request.Method);
            var restRequest = new RestRequest(request.Resource, method, DataFormat.Json);

            foreach (var param in request.Parameters)
            {
                restRequest.AddParameter(param.Name, param.Value!, ParameterType.QueryString);
            }

            foreach (var header in request.Headers)
            {
                restRequest.AddHeader(header.Name, header.Value);
            }

            if (request.Body is not null)
            {
                var body = _serializer.Serialize(request.Body);
                restRequest.AddParameter(_serializer.ContentType, body, ParameterType.RequestBody);
            }

            return restRequest;
        }
    }
}
