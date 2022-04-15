using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;

namespace NClient.Testing.Common.Clients
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
