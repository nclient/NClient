using System.Collections.Generic;
using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    [Path("api/query")]
    public interface IQueryClientWithMetadata : IQueryClient
    {
        [GetMethod]
        new Task<int> GetAsync(int id);
        
        [GetMethod]
        new Task<IEnumerable<int>> GetAsync([QueryParam] IEnumerable<int> ids);
        
        [GetMethod]
        new Task<IDictionary<string, int>> GetAsync([QueryParam] IDictionary<string, int> keyValues);

        [PostMethod]
        new Task PostAsync([QueryParam] BasicEntity entity);

        [PutMethod]
        new Task PutAsync([QueryParam] BasicEntity entity);

        [DeleteMethod]
        new Task DeleteAsync(int id);
    }
}
