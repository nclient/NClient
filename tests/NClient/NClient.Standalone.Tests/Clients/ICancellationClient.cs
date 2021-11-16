using System.Threading;
using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Testing.Common.Clients;

namespace NClient.Standalone.Tests.Clients
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
