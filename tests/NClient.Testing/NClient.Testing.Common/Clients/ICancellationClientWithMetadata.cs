using System.Threading;
using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;

namespace NClient.Testing.Common.Clients
{
    [Path("api/cancellation")]
    public interface ICancellationClientWithMetadata : ICancellationClient
    {
        [GetMethod]
        new int Get(int id, CancellationToken cancellationToken);
        
        [GetMethod]
        new Task<int> GetAsync(int id, CancellationToken cancellationToken);
    }
}
