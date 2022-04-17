using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;

namespace NClient.Testing.Common.Clients
{
    [Path("api/authorization")]
    public interface IAuthorizationClientWithMetadata : IAuthorizationClient
    {
        [GetMethod]
        new Task<int> GetAsync(int id);
    }
}
