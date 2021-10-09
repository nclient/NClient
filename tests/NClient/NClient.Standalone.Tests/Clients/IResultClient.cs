using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Standalone.Results;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Standalone.Tests.Clients
{
    [Path("api/result")]
    public interface IResultClientWithMetadata : IResultClient
    {
        [GetMethod("ints")]
        new Task<IResult<int?, Error?>> GetIntAsync(int id);

        [GetMethod("entities")]
        new Task<IResult<BasicEntity?, Error?>> GetEntityAsync(int id);
    }
}
