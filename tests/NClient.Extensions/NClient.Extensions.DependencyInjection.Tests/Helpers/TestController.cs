using System.Threading.Tasks;
using NClient.Annotations.Methods;
using NClient.Testing.Common.Entities;

namespace NClient.Extensions.DependencyInjection.Tests.Helpers
{
    public interface ITestClientWithMetadata : ITestClient
    {
        [GetMethod]
        new Task<int> GetAsync(int id);

        [PostMethod]
        new Task PostAsync(BasicEntity entity);

        [PutMethod]
        new Task PutAsync(BasicEntity entity);

        [DeleteMethod]
        new Task DeleteAsync(int id);
    }

    public interface ITestClient
    {
        Task<int> GetAsync(int id);
        Task PostAsync(BasicEntity entity);
        Task PutAsync(BasicEntity entity);
        Task DeleteAsync(int id);
    }
}
