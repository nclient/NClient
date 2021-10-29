using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validator.Transport
{
    internal class StubTransportMessageBuilder : ITransportMessageBuilder<IRequest, IResponse>
    {
        public Task<IRequest> BuildTransportRequestAsync(IRequest request)
        {
            return Task.FromResult(request);
        }
        public Task<IResponse> BuildResponseAsync(IRequest request, IRequest transportRequest, IResponse transportResponse)
        {
            return Task.FromResult(transportResponse);
        }
    }
}
