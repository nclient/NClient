using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace NClient.Providers.Transport.RestSharp
{
    internal class RestSharpTransport : ITransport<IRestRequest, IRestResponse>
    {
        private readonly IRestClient _restClient;

        public TimeSpan Timeout => TimeSpan.FromMilliseconds(_restClient.Timeout);

        public RestSharpTransport(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<IRestResponse> ExecuteAsync(IRestRequest transportRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _restClient.ExecuteAsync(transportRequest, cancellationToken).ConfigureAwait(false);
        }
    }
}
