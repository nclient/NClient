using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Testing.Common.Clients;

namespace NClient.Standalone.Tests.Clients
{
    [Path("api/timeout")]
    public interface ITimeoutClientWithMetadata : ITimeoutClient
    {
        [GetMethod]
        new int Get(int id);
        
        [GetMethod]
        new Task<int> GetAsync(int id);
    }
}
