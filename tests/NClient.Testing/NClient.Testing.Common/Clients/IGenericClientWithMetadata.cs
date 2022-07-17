using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Testing.Common.Entities;

namespace NClient.Testing.Common.Clients
{
    public interface IGenericClientWithMetadata : IGenericClientWithMetadataBase<BasicEntity, int>
    {
    }

    [Path("api/generic")]
    public interface IGenericClientWithMetadataBase<TIn, TOut>
    {
        [PostMethod]
        Task<TOut> PostAsync(TIn id);
    }
}
