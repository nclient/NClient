using System.Threading.Tasks;
using NClient.InterfaceProxy.Attributes;
using NClient.InterfaceProxy.Attributes.Methods;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.InterfaceProxy.Tests.Clients
{
    [Api("api/rest")]
    public interface IRestClientWithMetadata : IRestClient
    {
        [AsHttpGet("{id}")]
        new Task<int> GetAsync(int id);

        [AsHttpPost]
        new Task PostAsync(BasicEntity entity);

        [AsHttpPut]
        new Task PutAsync(BasicEntity entity);

        [AsHttpDelete("{id}")]
        new Task DeleteAsync(int id);
    }
}
