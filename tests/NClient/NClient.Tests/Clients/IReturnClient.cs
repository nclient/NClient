using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Tests.Clients
{
    [Path("api")]
    public interface IReturnClientWithMetadata : IReturnClient
    {
        [GetMethod]
        new BasicEntity Get(int id);
        
        [GetMethod]
        new Task<BasicEntity> GetAsync(int id);

        [GetMethod]
        HttpResponse<BasicEntity> GetHttpResponse(int id);

        [PostMethod]
        new void Post(BasicEntity entity);
        
        [PostMethod]
        new Task PostAsync(BasicEntity entity);

        [PostMethod]
        HttpResponse PostHttpResponse(BasicEntity entity);
    }
}
