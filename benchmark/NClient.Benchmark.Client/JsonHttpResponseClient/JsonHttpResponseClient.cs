using System.Collections.Generic;
using System.Threading.Tasks;
using NClient.Annotations.Http;
using NClient.Providers.Results.HttpResults;

namespace NClient.Benchmark.Client.JsonHttpResponseClient
{
    public interface INClientJsonHttpResponseClient
    {
        [PostMethod("/api")]
        Task<IHttpResponse<List<string>>> SendAsync([BodyParam] string[] ids);
    }
    
    public interface IRefitJsonHttpResponseClient
    {
        [Refit.Post("/api")]
        Task<Refit.IApiResponse<List<string>>> SendAsync([Refit.Body] string[] ids);
    }
    
    public interface IRestEaseJsonHttpResponseClient
    {
        [RestEase.Post("/api")]
        Task<RestEase.Response<List<string>>> SendAsync([RestEase.Body] string[] ids);
    }
}
