using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    [Path("api/basic")]
    public interface IBasicClientWithMetadata : IBasicClient
    {
        [GetMethod]
        new Task<int> GetAsync(int id);

        [PostMethod]
        new Task PostAsync(BasicEntity entity);

        [PutMethod]
        new Task PutAsync(BasicEntity entity);

        [DeleteMethod]
        new Task DeleteAsync(int id);
    }
}
