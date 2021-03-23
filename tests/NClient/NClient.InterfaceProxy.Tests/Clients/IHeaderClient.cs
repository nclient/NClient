using System.Threading.Tasks;
using NClient.Core.Attributes;
using NClient.Core.Attributes.Methods;
using NClient.Core.Attributes.Parameters;
using NClient.Testing.Common.Clients;

namespace NClient.InterfaceProxy.Tests.Clients
{
    [Path("api/header")]
    public interface IHeaderClientWithMetadata : IHeaderClient
    {
        [GetMethod]
        new Task<int> GetAsync([HeaderParam] int id);

        [DeleteMethod]
        new Task DeleteAsync([HeaderParam] int id);
    }
}
