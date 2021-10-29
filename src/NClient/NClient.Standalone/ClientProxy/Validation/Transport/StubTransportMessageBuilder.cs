using System.Threading.Tasks;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubTransportMessageBuilder : ITransportMessageBuilder<IRequest, IResponse>
    {
        public Task<IRequest> BuildTransportRequestAsync(IRequest request)
        {
            return Task.FromResult(request);
        }
        public Task<IResponse> BuildResponseAsync(IRequest request, IResponseContext<IRequest, IResponse> responseContext)
        {
            return Task.FromResult(responseContext.Response);
        }
    }
}
