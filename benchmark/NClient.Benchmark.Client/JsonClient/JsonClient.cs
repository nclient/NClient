using System.Threading.Tasks;
using NClient.Annotations.Http;
using NClient.Benchmark.Client.Dtos;

namespace NClient.Benchmark.Client.JsonClient
{
    public interface IJsonClient
    {
        [PostMethod("/api")]
        [Refit.Post("/api")]
        [RestEase.Post("/api")]
        Task<Dto> SendAsync([BodyParam, Refit.Body, RestEase.Body] Dto dto);
    }
}
