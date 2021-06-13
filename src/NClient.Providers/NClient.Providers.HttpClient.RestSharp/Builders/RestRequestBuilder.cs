using System;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
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

        public RestRequestBuilder(ISerializer serializer)
        {
            _serializer = serializer;
        }
        
        public IRestRequest Build(HttpRequest request)
        {
            Enum.TryParse(request.Method.ToString(), out Method method);
            var restRequest = new RestRequest(request.Uri, method, DataFormat.Json);

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