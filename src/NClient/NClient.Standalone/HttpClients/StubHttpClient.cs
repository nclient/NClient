using System.Net;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;

namespace NClient.Standalone.HttpClients
{
    internal class StubHttpClient : IHttpClient<IHttpRequest, object>
    {
        public Task<object> ExecuteAsync(IHttpRequest request)
        {
            return Task.FromResult((object)null);
        }
    }
}
