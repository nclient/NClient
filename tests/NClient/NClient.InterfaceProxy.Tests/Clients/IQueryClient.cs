using System.Threading.Tasks;
using NClient.Core.Attributes.Clients;
using NClient.Core.Attributes.Clients.Methods;
using NClient.Core.Attributes.Clients.Parameters;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.InterfaceProxy.Tests.Clients
{
    [Api("api/query")]
    public interface IQueryClientWithMetadata : IQueryClient
    {
        [AsHttpGet]
        new Task<int> GetAsync(int id);

        [AsHttpPost]
        new Task PostAsync([ToQuery] BasicEntity entity);

        [AsHttpPut]
        new Task PutAsync([ToQuery] BasicEntity entity);

        [AsHttpDelete]
        new Task DeleteAsync(int id);
    }
}
