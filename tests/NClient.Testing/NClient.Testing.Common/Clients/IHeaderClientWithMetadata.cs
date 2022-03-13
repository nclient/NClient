using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;

namespace NClient.Testing.Common.Clients
{
    [Path("api/header")]
    public interface IHeaderClientWithMetadata : IHeaderClient
    {
        [GetMethod]
        new Task<int> GetAsync([HeaderParam] int id);

        [DeleteMethod]
        new Task DeleteAsync([HeaderParam] int id);

        [DeleteMethod, Header("id", "1")]
        Task DeleteAsync();
    }
}
