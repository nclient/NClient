using System.Collections.Generic;
using System.Threading.Tasks;
using NClient.Annotations.Http;
using NClient.Providers.Results.HttpResults;

namespace NClient.Benchmark.Client.JsonHttpResponseApi
{
    public interface INClientJsonHttpResponseApiClient
    {
        [PostMethod("/api")]
        Task<IHttpResponse<List<string>>> SendAsync([BodyParam] string[] ids);
    }
    
    public interface IRefitJsonHttpResponseApiClient
    {
        [Refit.Post("/api")]
        Task<Refit.IApiResponse<List<string>>> SendAsync([Refit.Body] string[] ids);
    }
    
    public interface IRestEaseJsonHttpResponseApiClient
    {
        [RestEase.Post("/api")]
        Task<RestEase.Response<List<string>>> SendAsync([RestEase.Body] string[] ids);
    }
}
