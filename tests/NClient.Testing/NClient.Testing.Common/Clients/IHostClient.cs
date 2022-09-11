using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;

namespace NClient.Testing.Common.Clients
{
    [Path("path")]
    public interface IHostClient : INClient
    {
        [GetMethod]
        Task<int> GetAsync(int id);
    }
}
