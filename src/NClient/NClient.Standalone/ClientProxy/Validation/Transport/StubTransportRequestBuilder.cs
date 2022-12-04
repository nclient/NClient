using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubTransportRequestBuilder<TRequest, TResponse> : ITransportRequestBuilder<IRequest, IResponse>
    {
        public Task<IRequest> BuildAsync(IRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request);
        }
        public Task<IResponse> BuildResponseAsync(IResponseContext<IRequest, IResponse> responseContext, 
            CancellationToken cancellationToken)
        {
            return Task.FromResult<IResponse>(responseContext.Response);
        }
    }
}
