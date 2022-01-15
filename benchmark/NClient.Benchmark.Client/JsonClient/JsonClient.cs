using System.Collections.Generic;
using System.Threading.Tasks;
using NClient.Annotations.Http;

namespace NClient.Benchmark.Client.JsonClient
{
    public interface IJsonClient
    {
        [PostMethod("/api")]
        [Refit.Post("/api")]
        [RestEase.Post("/api")]
        Task<List<string>> SendAsync([BodyParam, Refit.Body, RestEase.Body] string[] ids);
    }
}
