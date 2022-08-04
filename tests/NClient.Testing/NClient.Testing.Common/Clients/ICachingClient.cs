using System.Threading.Tasks;

namespace NClient.Testing.Common.Clients
{
    public interface ICachingClient : INClient
    {
        int Get(int id);
        Task<int> GetAsync(int id);
    }
}
