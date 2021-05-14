using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Testing.Common.Clients;

namespace NClient.Tests.Clients
{
    [Path("api/header")]
    public interface IHeaderClientWithMetadata : IHeaderClient
    {
        [GetMethod]
        new Task<int> GetAsync([HeaderParam] int id);

        [DeleteMethod]
        new Task DeleteAsync([HeaderParam] int id);

        [DeleteMethod, Header("id", "1")]
        new Task DeleteAsync();
    }
}
