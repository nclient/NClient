using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Testing.Common.Clients;

namespace NClient.Standalone.Tests.Clients
{
    [Path("api/timeout")]
    [Timeout(2)]
    public interface ITimeoutClientWithMetadata : ITimeoutClient
    {
        [GetMethod]
        [Timeout(1)]
        new int Get(int id);
        
        [GetMethod]
        new Task<int> GetAsync(int id);
    }
}
