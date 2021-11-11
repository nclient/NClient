using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Providers.Mapping.Results;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Standalone.Tests.Clients
{
    [Path("api/result")]
    public interface IResultClientWithMetadata : IResultClient
    {
        [GetMethod("ints")]
        Task<IResult<int?, Error?>> GetIResultWithIntAsync(int id);
        
        [GetMethod("ints")]
        Task<Result<int?, Error?>> GetResultWithIntAsync(int id);

        [GetMethod("entities")]
        Task<IResult<BasicEntity?, Error?>> GetIResultWithEntityAsync(int id);
        
        [GetMethod("entities")]
        Task<Result<BasicEntity?, Error?>> GetResultWithEntityAsync(int id);
    }
}
