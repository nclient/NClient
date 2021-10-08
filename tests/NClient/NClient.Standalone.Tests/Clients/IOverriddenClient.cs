using System.Threading.Tasks;
using NClient.Abstractions;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Providers.Results.HttpMessages;
using NClient.Testing.Common.Entities;

namespace NClient.Standalone.Tests.Clients
{
    public interface IOverriddenClientWithMetadata : IOverriddenClientWithMetadataBase
    {
        [Override]
        new Task<IHttpResponse<int>> GetAsync(int id);

        [Override]
        [PutMethod("fakePost")]
        new Task<IHttpResponse> PostAsync(BasicEntity entity);

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
