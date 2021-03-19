using System.Threading.Tasks;
using NClient.Core.Attributes.Clients;
using NClient.Core.Attributes.Clients.Methods;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.InterfaceProxy.Tests.Clients
{
    [Api("api/basic")]
    public interface IBasicClientWithMetadata : IBasicClient
    {
        [AsHttpGet]
        new Task<int> GetAsync(int id);

        [AsHttpPost]
        new Task PostAsync(BasicEntity entity);

        [AsHttpPut]
        new Task PutAsync(BasicEntity entity);

        [AsHttpDelete]
        new Task DeleteAsync(int id);
    }
}
