using System.Threading;
using System.Threading.Tasks;
using NClient.Annotations.Http;
using NClient.Testing.Common.Clients;

namespace NClient.Standalone.Tests.Clients
{
    public interface ICancellationClientWithMetadata : ICancellationClient
    {
        [GetMethod]
        new int Get(int id, CancellationToken cancellationToken);
        
        [GetMethod]
        new Task<int> GetAsync(int id, CancellationToken cancellationToken);
    }
}
