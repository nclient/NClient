using System.Threading.Tasks;
using NClient.Annotations.Http;
using NClient.Benchmark.Client.Dtos;
using NClient.Providers.Results.HttpResults;

namespace NClient.Benchmark.Client.JsonHttpResponseClient
{
    public interface INClientJsonHttpResponseClient
    {
        [PostMethod("/api")]
        Task<IHttpResponse<Dto>> SendAsync([BodyParam] Dto dto);
    }
    
    public interface IRefitJsonHttpResponseClient
    {
        [Refit.Post("/api")]
        Task<Refit.IApiResponse<Dto>> SendAsync([Refit.Body] Dto dto);
    }
    
    public interface IRestEaseJsonHttpResponseClient
    {
        [RestEase.Post("/api")]
        Task<RestEase.Response<Dto>> SendAsync([RestEase.Body] Dto dto);
    }
}
