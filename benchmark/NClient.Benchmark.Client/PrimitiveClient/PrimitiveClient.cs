using System.Threading.Tasks;
using NClient.Annotations.Http;

namespace NClient.Benchmark.Client.PrimitiveClient
{
    public interface IPrimitiveClient
    {
        [GetMethod("/api")]
        [Refit.Get("/api")]
        [RestEase.Get("/api")]
        Task<int> SendAsync([QueryParam, Refit.Query, RestEase.Query] int id);
    }
}
