using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Tests.Clients
{
    [Path("api/sync")]
    public interface ISyncClientWithMetadata : ISyncClient
    {
        [GetMethod]
        new int Get(int id);

        [PostMethod]
        new void Post(BasicEntity entity);

        [PutMethod]
        new void Put(BasicEntity entity);

        [DeleteMethod]
        new void Delete(int id);
    }
}
