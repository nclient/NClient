using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NClient.Abstractions.Providers.HttpClient;
using NClient.Abstractions.Providers.Serialization;
using NClient.Providers.Transport.Http.RestSharp.Builders;
using NClient.Providers.Transport.Http.RestSharp.Helpers;
using ParameterType = RestSharp.ParameterType;
using DataFormat = RestSharp.DataFormat;
using RestRequest = RestSharp.RestRequest;
using IRestRequest = RestSharp.IRestRequest;
using IRestResponse = RestSharp.IRestResponse;

namespace NClient.Providers.Transport.Http.RestSharp
{
    internal class RestSharpHttpMessageBuilder : IHttpMessageBuilder<IRestRequest, IRestResponse>
    {
        private static readonly HashSet<string> ContentHeaderNames = new(new HttpContentKnownHeaderNames());

        private readonly ISerializer _serializer;
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;
        private readonly IFinalHttpRequestBuilder _finalHttpRequestBuilder;

        public RestSharpHttpMessageBuilder(
            ISerializer serializer,
            IRestSharpMethodMapper restSharpMethodMapper,
            IFinalHttpRequestBuilder finalHttpRequestBuilder)
        {
            _serializer = serializer;
            _restSharpMethodMapper = restSharpMethodMapper;
            _finalHttpRequestBuilder = finalHttpRequestBuilder;
        }

        public Task<IRestRequest> BuildRequestAsync(IHttpRequest httpRequest)
        {
            var method = _restSharpMethodMapper.Map(httpRequest.Method);
            var restRequest = new RestRequest(httpRequest.Resource, method, DataFormat.Json);

            foreach (var param in httpRequest.Parameters)
            {
                restRequest.AddParameter(param.Name, param.Value!, ParameterType.QueryString);
            }

            restRequest.AddHeader(HttpKnownHeaderNames.Accept, MediaTypeWithQualityHeaderValue.Parse(_serializer.ContentType).ToString());
            
            foreach (var header in httpRequest.Headers)
            {
                restRequest.AddHeader(header.Name, header.Value);
            }

            if (httpRequest.Data is not null)
            {
                var body = _serializer.Serialize(httpRequest.Data);
                restRequest.AddParameter(_serializer.ContentType, body, ParameterType.RequestBody);
            }

            return Task.FromResult((IRestRequest)restRequest);
        }
        
        public Task<IHttpResponse> BuildResponseAsync(IHttpRequest httpRequest, IRestRequest request, IRestResponse response)
        {
            var finalRequest = _finalHttpRequestBuilder.Build(httpRequest, response.Request);
            
            var allHeaders = response.Headers
                .Where(x => x.Name != null)
                .Select(x => new HttpHeader(x.Name!, x.Value?.ToString() ?? ""))
                .ToArray();
            var responseHeaders = allHeaders
                .Where(x => !ContentHeaderNames.Contains(x.Name))
                .ToArray();
            var contentHeaders = allHeaders
                .Where(x => ContentHeaderNames.Contains(x.Name))
                .ToArray();
            
            var httpResponse = new HttpResponse(finalRequest)
            {
                Content = new HttpResponseContent(response.RawBytes, new HttpResponseContentHeaderContainer(contentHeaders)),
                StatusCode = response.StatusCode,
                ResponseUri = response.ResponseUri,
                Headers = new HttpResponseHeaderContainer(responseHeaders),
                ErrorMessage = response.ErrorMessage,
                ErrorException = response.ErrorException,
                ProtocolVersion = response.ProtocolVersion
            };

            return Task.FromResult<IHttpResponse>(httpResponse);
        }
    }
}
