using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubTransportBuilder : ITransport<IRequest, IResponse>
    {
        public TimeSpan Timeout => System.Threading.Timeout.InfiniteTimeSpan;
        
        public Task<IResponse> ExecuteAsync(IRequest transportRequest, CancellationToken cancellationToken)
        {
            var response = new Response(transportRequest) { StatusCode = 200 };
            return Task.FromResult<IResponse>(response);
        }
    }
}
