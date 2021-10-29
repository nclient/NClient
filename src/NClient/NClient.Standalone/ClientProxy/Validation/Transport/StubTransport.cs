using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubTransport : ITransport<IRequest, IResponse>
    {
        public Task<IResponse> ExecuteAsync(IRequest transportRequest)
        {
            var response = new Response(transportRequest) { StatusCode = 200 };
            return Task.FromResult<IResponse>(response);
        }
    }
}
