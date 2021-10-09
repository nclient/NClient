using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Providers.Results.HttpMessages;

namespace NClient.Standalone.HttpClients
{
    internal class StubHttpClient : IHttpClient<IHttpRequest, IHttpResponse>
    {
        public Task<IHttpResponse> ExecuteAsync(IHttpRequest httpRequest)
        {
            return Task.FromResult<IHttpResponse>(new HttpResponse(httpRequest));
        }
    }
}
