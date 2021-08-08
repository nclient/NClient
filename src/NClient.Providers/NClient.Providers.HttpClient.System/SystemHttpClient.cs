using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NClient.Abstractions.HttpClients;
using NClient.Common.Helpers;
using NClient.Providers.HttpClient.System.Builders;

namespace NClient.Providers.HttpClient.System
{
    internal class SystemHttpClient : IHttpClient
    {
        private readonly IHttpRequestMessageBuilder _httpRequestMessageBuilder;
        private readonly IHttpResponseBuilder _httpResponseBuilder;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _httpClientName;

        public SystemHttpClient(
            IHttpRequestMessageBuilder httpRequestMessageBuilder,
            IHttpResponseBuilder httpResponseBuilder,
            IHttpClientFactory httpClientFactory,
            string? httpClientName = null)
        {
            _httpRequestMessageBuilder = httpRequestMessageBuilder;
            _httpResponseBuilder = httpResponseBuilder;
            _httpClientFactory = httpClientFactory;
            _httpClientName = httpClientName ?? Options.DefaultName;
        }

        public async Task<HttpResponse> ExecuteAsync(HttpRequest request)
        {
            Ensure.IsNotNull(request, nameof(request));

            using var httpRequestMessage = _httpRequestMessageBuilder.Build(request);
            var (httpResponseMessage, exception) = await TrySendAsync(httpRequestMessage).ConfigureAwait(false);

            try
            {
                return await _httpResponseBuilder
                    .BuildAsync(request, httpResponseMessage, exception)
                    .ConfigureAwait(false);
            }
            finally
            {
                httpRequestMessage.Dispose();
            }
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
