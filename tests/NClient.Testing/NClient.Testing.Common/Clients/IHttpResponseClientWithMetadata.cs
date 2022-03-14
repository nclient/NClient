using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Providers.Results.HttpResults;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    [Path("api/http")]
    public interface IHttpResponseClientWithMetadata : IHttpResponseClient
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
