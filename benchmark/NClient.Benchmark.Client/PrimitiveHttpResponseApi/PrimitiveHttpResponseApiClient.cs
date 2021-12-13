using System.Threading.Tasks;
using NClient.Annotations.Http;
using NClient.Providers.Results.HttpResults;

namespace NClient.Benchmark.Client.PrimitiveHttpResponseApi
{
    public interface INClientPrimitiveHttpResponseApiClient
    {
        [GetMethod("/api")]
        Task<IHttpResponse<int>> SendAsync([QueryParam] int id);
    }
    
    public interface IRefitPrimitiveHttpResponseApiClient
    {
        [Refit.Get("/api")]
        Task<Refit.IApiResponse<int>> SendAsync([Refit.Query] int id);
    }
    
    public interface IRestEasePrimitiveHttpResponseApiClient
    {
        [RestEase.Get("/api")]
        Task<RestEase.Response<int>> SendAsync([RestEase.Query] int id);
    }
}
