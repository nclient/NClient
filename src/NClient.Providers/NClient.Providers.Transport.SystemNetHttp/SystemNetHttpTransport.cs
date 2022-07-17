using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NClient.Common.Helpers;

namespace NClient.Providers.Transport.SystemNetHttp
{
    internal class SystemNetHttpTransport : ITransport<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly HttpClient _httpClient;
        
        public TimeSpan Timeout => _httpClient.Timeout;

        public SystemNetHttpTransport(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage transportRequest, CancellationToken cancellationToken)
        {
            Ensure.IsNotNull(transportRequest, nameof(transportRequest));
            cancellationToken.ThrowIfCancellationRequested();

            return await _httpClient.SendAsync(transportRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        }
    }
}
