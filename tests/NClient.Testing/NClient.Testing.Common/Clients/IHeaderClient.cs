using System.Threading.Tasks;

namespace NClient.Testing.Common.Clients
{
    public interface IHeaderClient : INClient
    {
        Task<int> GetWithSingleHeaderAsync(int id);
        
        Task<int[]> GetWithMultipleHeaderValuesAsync(int id1, int id2);
        
        Task<int[]> GetWithMultipleHeadersAsync(int id1, int id2);

        Task<int> GetWithSingleStaticHeaderAsync();
        
        Task<int[]> GetWithMultipleStaticHeaderValuesAsync();
        
        Task<int[]> GetWithMultipleStaticHeadersAsync();
        
        Task<int[]> GetWithMultipleStaticAndParamHeadersAsync(int id1, int id2);
    }
}
