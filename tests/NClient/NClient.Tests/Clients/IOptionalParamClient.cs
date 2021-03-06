using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Tests.Clients
{
    [Path("api/optionalParam")]
    public interface IOptionalParamWithMetadata : IOptionalParamClient
    {
        [GetMethod]
        new Task<int> GetAsync(int id = 1);

        [PostMethod]
        new Task PostAsync(BasicEntity? entity = null);
    }
}
