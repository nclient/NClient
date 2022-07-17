using System.Threading.Tasks;

namespace NClient.Testing.Common.Clients
{
    public interface IAuthorizationClient : INClient
    {
        Task<int> GetAsync(int id);
    }
}
