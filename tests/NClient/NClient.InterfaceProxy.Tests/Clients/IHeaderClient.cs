using System.Threading.Tasks;
using NClient.Core.Attributes.Clients;
using NClient.Core.Attributes.Clients.Methods;
using NClient.Core.Attributes.Clients.Parameters;
using NClient.Testing.Common.Clients;

namespace NClient.InterfaceProxy.Tests.Clients
{
    [Client("api/header")]
    public interface IHeaderClientWithMetadata : IHeaderClient
    {
        [AsHttpGet]
        new Task<int> GetAsync([ToHeader] int id);

        [AsHttpDelete]
        new Task DeleteAsync([ToHeader] int id);
    }
}
