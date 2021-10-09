using System.Net;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;

namespace NClient.Standalone.Client.HttpClients
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
