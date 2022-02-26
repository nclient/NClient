using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    [Path("api/http")]
    public interface IHttpClientWithMetadata : IHttpClient
    {
        [GetMethod]
        new Task<IResponse<int>> GetAsync(int id);

        [PostMethod]
        new Task<IResponse<BasicEntity>> PostAsync(BasicEntity entity);

        [PutMethod]
        new Task<IResponse> PutAsync(BasicEntity entity);

        [DeleteMethod]
        new IResponse Delete(int id);
    }
}
