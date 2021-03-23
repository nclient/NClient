using System.Threading.Tasks;
using NClient.Core.Attributes;
using NClient.Core.Attributes.Methods;
using NClient.Core.Attributes.Parameters;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.InterfaceProxy.Tests.Clients
{
    [Path("api/query")]
    public interface IQueryClientWithMetadata : IQueryClient
    {
        [GetMethod]
        new Task<int> GetAsync(int id);

        [PostMethod]
        new Task PostAsync([QueryParam] BasicEntity entity);

        [PutMethod]
        new Task PutAsync([QueryParam] BasicEntity entity);

        [DeleteMethod]
        new Task DeleteAsync(int id);
    }
}
