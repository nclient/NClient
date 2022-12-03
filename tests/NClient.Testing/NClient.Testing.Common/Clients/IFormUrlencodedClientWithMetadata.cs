using System.Collections.Generic;
using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    [Path("api/formUrlencoded")]
    public interface IFormUrlencodedClientWithMetadata : IFormUrlencodedClient
    {
        [PostMethod]
        new Task<IResponse> PostAsync([FormParam] int id);
        
        [PostMethod]
        new Task<IResponse> PostAsync([FormParam] IDictionary<string, object> dict);
        
        [PostMethod]
        new Task<IResponse> PostAsync([FormParam] BasicEntity entity);
    }
}
