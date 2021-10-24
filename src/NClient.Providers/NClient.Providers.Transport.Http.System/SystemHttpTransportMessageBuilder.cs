using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using NClient.Providers.Serialization;
using NClient.Providers.Transport.Http.System.Builders;
using NClient.Providers.Transport.Http.System.Helpers;

namespace NClient.Providers.Transport.Http.System
{
    internal class SystemHttpTransportMessageBuilder : ITransportMessageBuilder<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISerializer _serializer;
        private readonly ISystemHttpMethodMapper _systemHttpMethodMapper;
        private readonly IFinalHttpRequestBuilder _finalHttpRequestBuilder;

        public SystemHttpTransportMessageBuilder(
            ISerializer serializer,
            ISystemHttpMethodMapper systemHttpMethodMapper,
            IFinalHttpRequestBuilder finalHttpRequestBuilder)
        {
            _serializer = serializer;
            _systemHttpMethodMapper = systemHttpMethodMapper;
            _finalHttpRequestBuilder = finalHttpRequestBuilder;
        }
        
        public Task<HttpRequestMessage> BuildTransportRequestAsync(IRequest request)
        {
            var parameters = request.Parameters
                .ToDictionary(x => x.Name, x => x.Value!.ToString());
            var uri = new Uri(QueryHelpers.AddQueryString(request.Resource.ToString(), parameters));

            var httpRequestMessage = new HttpRequestMessage
            {
                // TODO: как быть?
                Method = _systemHttpMethodMapper.Map(request.Method.Value), 
                RequestUri = uri
            };

            httpRequestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(_serializer.ContentType));

            foreach (var header in request.Headers)
            {
                httpRequestMessage.Headers.Add(header.Name, header.Value);
            }
            
            if (request.Data != null)
            {
                var body = _serializer.Serialize(request.Data);
                httpRequestMessage.Content = new StringContent(body, Encoding.UTF8, _serializer.ContentType);
            }

            return Task.FromResult(httpRequestMessage);
        }

        public async Task<IResponse> BuildResponseAsync(IRequest transportRequest, HttpRequestMessage request, HttpResponseMessage response)
        {
            var exception = TryGetException(response);
            
            var finalHttpRequest = await _finalHttpRequestBuilder
                .BuildAsync(transportRequest, response.RequestMessage)
                .ConfigureAwait(false);

            var content = response.Content is null 
                ? new Content() 
                : new Content(
                    await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false),
                    new ContentHeaderContainer(response.Content.Headers));

            var httpResponse = new Response(finalHttpRequest)
            {
                Headers = new HeaderContainer(response.Headers),
                Content = content,
                StatusCode = (int)response.StatusCode,
                StatusDescription = response.StatusCode.ToString(),
                ResponseUri = response.RequestMessage.RequestUri,
                ErrorMessage = exception?.Message,
                ErrorException = exception,
                ProtocolVersion = response.Version,
                IsSuccessful = response.IsSuccessStatusCode
            };

            return httpResponse;
        }

        private static HttpRequestException? TryGetException(HttpResponseMessage httpResponseMessage)
        {
            try
            {
                httpResponseMessage.EnsureSuccessStatusCode();
                return null;
            }
            catch (HttpRequestException e)
            {
                return e;
            }
        }
    }
}
