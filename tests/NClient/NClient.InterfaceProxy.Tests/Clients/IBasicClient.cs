using System.Threading.Tasks;
using NClient.Core.Attributes;
using NClient.Core.Attributes.Methods;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.InterfaceProxy.Tests.Clients
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
