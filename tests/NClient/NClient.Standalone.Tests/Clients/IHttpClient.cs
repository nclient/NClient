using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;
using IHttpClient = NClient.Testing.Common.Clients.IHttpClient;

namespace NClient.Standalone.Tests.Clients
{
    [Path("api/http")]
    public interface IHttpClientWithMetadata : IHttpClient
    {
        [GetMethod]
        new Task<IHttpResponse<int>> GetAsync(int id);

        [PostMethod]
        new Task<IHttpResponse<BasicEntity>> PostAsync(BasicEntity entity);

        [PutMethod]
        new Task<IHttpResponse> PutAsync(BasicEntity entity);

        [DeleteMethod]
        new IHttpResponse Delete(int id);
    }
}
