using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Testing.Common.Entities;
using IHttpClient = NClient.Testing.Common.Clients.IHttpClient;

namespace NClient.Tests.Clients
{
    [Path("api/http")]
    public interface IHttpClientWithMetadata : IHttpClient
    {
        [GetMethod]
        new Task<HttpValueResponse<int>> GetAsync(int id);

        [PostMethod]
        new Task<HttpValueResponse<BasicEntity>> PostAsync(BasicEntity entity);

        [PutMethod]
        new Task<HttpResponse> PutAsync(BasicEntity entity);

        [DeleteMethod]
        new HttpResponse Delete(int id);
    }
}
