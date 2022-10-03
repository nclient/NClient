using System.Collections.Generic;
using System.Threading.Tasks;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface IFormUrlencodedClient : INClient
    {
        Task<int> PostAsync(int id);
        Task<int> PostAsync(IDictionary<string, object> dict);
        Task<int> PostAsync(BasicEntity entity);
    }
}
