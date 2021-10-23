using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NClient.Abstractions.Providers.HttpClient;
using NClient.Common.Helpers;

namespace NClient.Providers.Transport.Http.System
{
    internal class SystemHttpClient : IHttpClient<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _httpClientName;

        public SystemHttpClient(IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientName = httpClientName ?? Options.DefaultName;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage request)
        {
            Ensure.IsNotNull(request, nameof(request));

            var httpClient = _httpClientFactory.CreateClient(_httpClientName);
            return await httpClient.SendAsync(request).ConfigureAwait(false);
        }
    }
}
