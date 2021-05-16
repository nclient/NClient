using System;
using NClient.Abstractions.HttpClients;
using RestSharp;

namespace NClient.Providers.HttpClient.RestSharp.Internals
{
    public class RestRequestBuilder
    {
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
                restRequest.AddJsonBody(request.Body);
            }

            return restRequest;
        }
    }
}