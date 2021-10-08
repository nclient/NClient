using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Providers.Results.HttpMessages;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Standalone.Tests.Clients
{
    [Path("api")]
    public interface IReturnClientWithMetadata : IReturnClient
    {
        [GetMethod]
        new BasicEntity Get(int id);
        
        [GetMethod]
        new Task<BasicEntity> GetAsync(int id);

        [GetMethod]
        IHttpResponse<BasicEntity> GetHttpResponse(int id);

        [PostMethod]
        new void Post(BasicEntity entity);
        
        [PostMethod]
        new Task PostAsync(BasicEntity entity);

        [PostMethod]
        IHttpResponse PostHttpResponse(BasicEntity entity);
    }
}
