using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;

namespace NClient.Testing.Common.Clients
{
    [Path("api/timeout")]
    [Timeout(500)]
    public interface ITimeoutStaticClientWithMetadata : ITimeoutClient
    {
        [GetMethod]
        [Timeout(1000)]
        new int Get(int id);
        
        [GetMethod]
        new Task<int> GetAsync(int id);
    }
}
