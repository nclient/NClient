using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.InterfaceProxy.Tests.Clients
{
    [Path("api/rest")]
    public interface IRestClientWithMetadata : IRestClient
    {
        [GetMethod("{id}")]
        new Task<int> GetAsync(int id);

        [PostMethod]
        new Task PostAsync(BasicEntity entity);

        [PutMethod]
        new Task PutAsync(BasicEntity entity);

        [DeleteMethod("{id}")]
        new Task DeleteAsync(int id);
    }
}
