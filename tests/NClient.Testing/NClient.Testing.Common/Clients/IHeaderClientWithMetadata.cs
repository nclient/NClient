using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;

namespace NClient.Testing.Common.Clients
{
    [Path("api/header")]
    public interface IHeaderClientWithMetadata : IHeaderClient
    {
        [GetMethod]
        new Task<int> GetWithSingleHeaderAsync([HeaderParam] int id);
        
        [GetMethod]
        new Task<int[]> GetWithMultipleHeaderValuesAsync([HeaderParam("id")] int id1, [HeaderParam("id")] int id2);
        
        [GetMethod]
        new Task<int[]> GetWithMultipleHeadersAsync([HeaderParam] int id1, [HeaderParam] int id2);

        [GetMethod, Header("id", "1")]
        new Task<int> GetWithSingleStaticHeaderAsync();

        [GetMethod, Header("id", "1"), Header("id", "2")]
        new Task<int[]> GetWithMultipleStaticHeaderValuesAsync();
        
        [GetMethod, Header("id1", "1"), Header("id2", "2")]
        new Task<int[]> GetWithMultipleStaticHeadersAsync();
        
        [GetMethod, Header("id1", "1"), Header("id2", "2")]
        new Task<int[]> GetWithMultipleStaticAndParamHeadersAsync([HeaderParam] int id1, [HeaderParam] int id2);
    }
}
