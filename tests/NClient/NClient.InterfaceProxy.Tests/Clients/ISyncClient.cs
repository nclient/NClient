using NClient.Core.Attributes.Clients;
using NClient.Core.Attributes.Clients.Methods;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.InterfaceProxy.Tests.Clients
{
    [Client("api/sync")]
    public interface ISyncClientWithMetadata : ISyncClient
    {
        [AsHttpGet]
        new int Get(int id);

        [AsHttpPost]
        new void Post(BasicEntity entity);

        [AsHttpPut]
        new void Put(BasicEntity entity);

        [AsHttpDelete]
        new void Delete(int id);
    }
}
