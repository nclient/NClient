using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NClient.Common.Helpers;
using NClient.Providers.Transport.SystemNetHttp.Helpers;

namespace NClient.Providers.Transport.SystemNetHttp
{
    internal class SystemNetHttpTransport : ITransport<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _httpClientName;

        public SystemNetHttpTransport(IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientName = httpClientName ?? Options.DefaultName;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage transportRequest, CancellationToken cancellationToken)
        {
            Ensure.IsNotNull(transportRequest, nameof(transportRequest));
            cancellationToken.ThrowIfCancellationRequested();

            var httpClient = _httpClientFactory.CreateClient(_httpClientName);
            if (transportRequest.TryGetTimeout() is { } timeout && httpClient.Timeout != timeout)
                httpClient.Timeout = timeout;
            if (transportRequest.TryGetTimeout() is null)
                transportRequest.SetTimeout(httpClient.Timeout);
            
            return await httpClient.SendAsync(transportRequest, cancellationToken).ConfigureAwait(false);
        }
    }
}
