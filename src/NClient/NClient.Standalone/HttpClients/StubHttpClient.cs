using System.Net;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;

namespace NClient.Standalone.HttpClients
{
    internal class StubHttpClient : IHttpClient<HttpRequest, HttpResponse>
    {
        public Task<HttpResponse> ExecuteAsync(HttpRequest request)
        {
            var response = new HttpResponse(request) { StatusCode = HttpStatusCode.OK };
            return Task.FromResult(response);
        }
    }
}
