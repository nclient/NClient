using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface IOverriddenClientWithMetadata : IOverriddenClientWithMetadataBase
    {
        [Override]
        new Task<IResponse<int>> GetAsync(int id);

        [Override]
        [PutMethod("fakePost")]
        new Task<IResponse> PostAsync(BasicEntity entity);

        [Override]
        [Header("test", "2")]
        new Task PutAsync(BasicEntity entity);

        [Override]
        new Task<string> DeleteAsync([BodyParam] int id);
    }

    [Path("api/overridden")]
    public interface IOverriddenClientWithMetadataBase : INClient
    {
        [GetMethod]
        Task<int> GetAsync(int id);

        [PostMethod]
        Task PostAsync(BasicEntity entity);

        [PutMethod]
        [Header("test", "1")]
        Task PutAsync([BodyParam] BasicEntity entity);

        [DeleteMethod]
        Task DeleteAsync([QueryParam] int id);
    }
}
