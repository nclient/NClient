using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
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
