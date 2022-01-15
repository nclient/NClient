using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Testing.Common.Clients;

namespace NClient.Standalone.Tests.Clients
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
