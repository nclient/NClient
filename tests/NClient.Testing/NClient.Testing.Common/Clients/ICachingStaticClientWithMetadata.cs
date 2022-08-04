using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    [Path("api/caching")]
    [Caching(500)]
    public interface ICachingStaticClientWithMetadata : ITimeoutClient
    {
        [GetMethod]
        [Caching(1000)]
        IResponse<BasicEntity> GetIResponse(int id);
        
        [GetMethod]
        Task<IResponse<BasicEntity>> GetIResponseAsync(int id);
    }
}
