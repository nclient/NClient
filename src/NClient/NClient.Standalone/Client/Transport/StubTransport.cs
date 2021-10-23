using System.Net;
using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Standalone.Client.Transport
{
    internal class StubTransport : ITransport<IHttpRequest, IHttpResponse>
    {
        public Task<IHttpResponse> ExecuteAsync(IHttpRequest request)
        {
            var response = new HttpResponse(request) { StatusCode = HttpStatusCode.OK };
            return Task.FromResult<IHttpResponse>(response);
        }
    }
}
