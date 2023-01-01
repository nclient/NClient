using System.Collections.Generic;
using System.Threading.Tasks;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface IFormUrlencodedClient : INClient
    {
        Task<IResponse> PostAsync(int id);
        Task<IResponse> PostAsync(IDictionary<string, object> dict);
        Task<IResponse> PostAsync(BasicEntity entity);
    }
}
