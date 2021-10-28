using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Standalone.Client.Transport
{
    internal class StubTransport : ITransport<IRequest, IResponse>
    {
        public Task<IResponse> ExecuteAsync(IRequest request)
        {
            var response = new Response(request) { StatusCode = 200 };
            return Task.FromResult<IResponse>(response);
        }
    }
}
