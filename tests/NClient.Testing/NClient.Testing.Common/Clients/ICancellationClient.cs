using System.Threading;
using System.Threading.Tasks;

namespace NClient.Testing.Common.Clients
{
    public interface ICancellationClient
    {
        int Get(int id, CancellationToken cancellationToken);
        Task<int> GetAsync(int id, CancellationToken cancellationToken);
    }
}
