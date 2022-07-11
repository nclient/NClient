using System.Threading.Tasks;
using NClient.Annotations.Http;

namespace NClient.Testing.Common.Clients
{
    // [Path("api/class")] - It is valid on 'Interface' declarations only.
    public class ClassClientWithMetadata
    {
        [GetMethod]
        public Task<int> GetAsync(int id)
        {
            return Task.FromResult(1);
        }
    }
}
