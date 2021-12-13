using System.Threading.Tasks;
using NClient.Annotations.Http;

namespace NClient.Benchmark.Client.PrimitiveApi
{
    public interface IPrimitiveApiClient
    {
        [GetMethod("/api")]
        [Refit.Get("/api")]
        [RestEase.Get("/api")]
        Task<int> SendAsync([QueryParam, Refit.Query, RestEase.Query] int id);
    }
}
