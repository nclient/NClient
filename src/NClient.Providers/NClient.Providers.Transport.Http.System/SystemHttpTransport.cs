﻿using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NClient.Common.Helpers;

namespace NClient.Providers.Transport.Http.System
{
    internal class SystemHttpTransport : ITransport<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _httpClientName;

        public SystemHttpTransport(IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientName = httpClientName ?? Options.DefaultName;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage transportRequest)
        {
            Ensure.IsNotNull(transportRequest, nameof(transportRequest));

            var httpClient = _httpClientFactory.CreateClient(_httpClientName);
            return await httpClient.SendAsync(transportRequest).ConfigureAwait(false);
        }
    }
}
