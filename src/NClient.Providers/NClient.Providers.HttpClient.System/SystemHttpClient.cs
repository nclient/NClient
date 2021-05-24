using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.HttpClient.System.Internals;

namespace NClient.Providers.HttpClient.System
{
    public class SystemHttpClient : IHttpClient
    {
        private readonly HttpRequestMessageBuilder _httpRequestMessageBuilder;
        private readonly HttpResponseBuilder _httpResponseBuilder;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _httpClientName;

        public SystemHttpClient(ISerializer serializer, IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            _httpRequestMessageBuilder = new HttpRequestMessageBuilder(serializer);
            _httpResponseBuilder = new HttpResponseBuilder(serializer);
            _httpClientFactory = httpClientFactory;
            _httpClientName = httpClientName ?? Options.DefaultName;
        }

        public async Task<HttpResponse> ExecuteAsync(HttpRequest request, Type? bodyType = null, Type? errorType = null)
        {
            var httpRequestMessage = _httpRequestMessageBuilder.Build(request);
            var (httpResponseMessage, exception) = await TrySendAsync(httpRequestMessage).ConfigureAwait(false);
            return await _httpResponseBuilder.BuildAsync(request, httpResponseMessage, bodyType, errorType, exception).ConfigureAwait(false);
        }

        private async Task<(HttpResponseMessage HttpResponseMessage, Exception? Exception)> TrySendAsync(HttpRequestMessage httpRequestMessage)
        {
            var httpClient = _httpClientFactory.CreateClient(_httpClientName);
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false); ;

            try
            {
                httpResponseMessage.EnsureSuccessStatusCode();
                return (httpResponseMessage, null);
            }
            catch (HttpRequestException e)
            {
                return (httpResponseMessage, e);
            }
        }
    }
}
