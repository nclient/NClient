using System.Threading.Tasks;
using NClient.InterfaceProxy.Attributes;
using NClient.InterfaceProxy.Attributes.Methods;
using NClient.InterfaceProxy.Attributes.Parameters;
using NClient.Testing.Common.Clients;

namespace NClient.InterfaceProxy.Tests.Clients
{
    [Api("api/header")]
    public interface IHeaderClientWithMetadata : IHeaderClient
    {
        [AsHttpGet]
        new Task<int> GetAsync([ToHeader] int id);

        [AsHttpDelete]
        new Task DeleteAsync([ToHeader] int id);
    }
}
