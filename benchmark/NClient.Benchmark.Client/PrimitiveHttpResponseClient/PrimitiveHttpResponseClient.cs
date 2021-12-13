using System.Threading.Tasks;
using NClient.Annotations.Http;
using NClient.Providers.Results.HttpResults;

namespace NClient.Benchmark.Client.PrimitiveHttpResponseClient
{
    public interface INClientPrimitiveHttpResponseClient
    {
        [GetMethod("/api")]
        Task<IHttpResponse<int>> SendAsync([QueryParam] int id);
    }
    
    public interface IRefitPrimitiveHttpResponseClient
    {
        [Refit.Get("/api")]
        Task<Refit.IApiResponse<int>> SendAsync([Refit.Query] int id);
    }
    
    public interface IRestEasePrimitiveHttpResponseClient
    {
        [RestEase.Get("/api")]
        Task<RestEase.Response<int>> SendAsync([RestEase.Query] int id);
    }
}
