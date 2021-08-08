using System.Threading.Tasks;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Testing.Common.Entities;

namespace NClient.Tests.Clients
{
    public interface IOverriddenClientWithMetadata : IOverriddenClientWithMetadataBase
    {
        [Override]
        new Task<HttpResponse<int>> GetAsync(int id);

        [Override]
        [PutMethod("fakePost")]
        new Task<HttpResponse> PostAsync(BasicEntity entity);

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
        new Task<int> GetAsync(int id);

        [PostMethod]
        new Task PostAsync(BasicEntity entity);

        [PutMethod]
        [Header("test", "1")]
        new Task PutAsync([BodyParam] BasicEntity entity);

        [DeleteMethod]
        new Task DeleteAsync([QueryParam] int id);
    }
}
