using System.Threading.Tasks;
using NClient.InterfaceProxy.Attributes;
using NClient.InterfaceProxy.Attributes.Methods;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.InterfaceProxy.Tests.Clients
{
    [Api("api")]
    public interface IReturnClientWithMetadata : IReturnClient
    {
        [AsHttpGet]
        new Task<BasicEntity> GetAsync(int id);

        [AsHttpGet]
        new BasicEntity Get(int id);

        [AsHttpPost]
        new Task PostAsync(BasicEntity entity);

        [AsHttpPost]
        new void Post(BasicEntity entity);
    }
}
