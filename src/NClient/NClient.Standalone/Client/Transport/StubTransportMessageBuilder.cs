using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Standalone.Client.Transport
{
    internal class StubTransportMessageBuilder : ITransportMessageBuilder<IRequest, IResponse>
    {
        public Task<IRequest> BuildTransportRequestAsync(IRequest request)
        {
            return Task.FromResult(request);
        }
        public Task<IResponse> BuildResponseAsync(IRequest transportRequest, IRequest request, IResponse response)
        {
            return Task.FromResult(response);
        }
    }
}
