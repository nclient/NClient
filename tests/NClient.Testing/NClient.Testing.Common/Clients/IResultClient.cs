using System.Threading.Tasks;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface IResultClient : INClient
    {
        Task<int> GetIntAsync(int id);
        
        Task<BasicEntity> GetEntityAsync(int id);
    }
}
