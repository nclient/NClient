using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Handling;

namespace NClient.Testing.Common.Helpers
{
    public class DelayClientHandler : IClientHandler<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly TimeSpan _delay;
        
        public DelayClientHandler(TimeSpan delay)
        {
            _delay = delay;
        }
        
        public async Task<HttpRequestMessage> HandleRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await Task.Delay(_delay, cancellationToken)
                .ContinueWith(_ => Task.CompletedTask)
                .ConfigureAwait(false);
            return request;
        }
            
        public Task<HttpResponseMessage> HandleResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            return Task.FromResult(response);
        }
    }
}
