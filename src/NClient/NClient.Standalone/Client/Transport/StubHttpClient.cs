using System.Net;
using System.Threading.Tasks;
using NClient.Abstractions.Providers.Transport;

namespace NClient.Standalone.Client.Transport
{
    internal class StubHttpClient : IHttpClient<IHttpRequest, IHttpResponse>
    {
        public Task<IHttpResponse> ExecuteAsync(IHttpRequest request)
        {
            var response = new HttpResponse(request) { StatusCode = HttpStatusCode.OK };
            return Task.FromResult<IHttpResponse>(response);
        }
    }
}
