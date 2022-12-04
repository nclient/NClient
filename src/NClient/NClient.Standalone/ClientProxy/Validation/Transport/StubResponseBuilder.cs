using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubResponseBuilder<TRequest, TResponse> : IResponseBuilder<IRequest, IResponse> 
    {
        public Task<IResponse> BuildAsync(IRequest request, 
            IResponseContext<IRequest, IResponse> responseContext, 
            bool allocateMemoryForContent, 
            CancellationToken cancellationToken)
        {
            return Task.FromResult<IResponse>(responseContext.Response);
        }
    }
}
