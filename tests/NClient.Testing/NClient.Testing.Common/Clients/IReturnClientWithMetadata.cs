using System.Net.Http;
using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    [Path("api")]
    public interface IReturnClientWithMetadata : IReturnClient
    {
        [GetMethod]
        new BasicEntity Get(int id);
        
        [GetMethod]
        new Task<BasicEntity> GetAsync(int id);

        [GetMethod]
        IResponse<BasicEntity> GetIHttpResponse(int id);
        
        [GetMethod]
        HttpResponseMessage GetHttpResponseMessage(int id);

        [PostMethod]
        new void Post(BasicEntity entity);
        
        [PostMethod]
        new Task PostAsync(BasicEntity entity);

        [PostMethod]
        IResponse PostHttpResponse(BasicEntity entity);
    }
}
