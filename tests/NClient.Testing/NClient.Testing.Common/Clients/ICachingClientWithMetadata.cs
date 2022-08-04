using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;

namespace NClient.Testing.Common.Clients
{
    [Path("api/caching")]
    public interface ICachingClientWithMetadata : ITimeoutClient
    {
        [GetMethod]
        new int Get(int id);
        
        [GetMethod]
        new Task<int> GetAsync(int id);
    }
}
