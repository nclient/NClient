using System.Threading.Tasks;

namespace NClient.Testing.Common.Clients
{
    public interface ITimeoutClient : INClient
    {
        int Get(int id);
        Task<int> GetAsync(int id);
    }
}
