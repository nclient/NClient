using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Clients
{
    public interface IHttpNClient<T> where T : INClient
    {
        HttpResponse GetHttpResponse(Expression<Action<T>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        HttpResponse<TResult> GetHttpResponse<TResult>(Expression<Func<T, TResult>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        Task<HttpResponse> GetHttpResponse(Expression<Func<T, Task>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        Task<HttpResponse<TResult>> GetHttpResponse<TResult>(Expression<Func<T, Task<TResult>>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
    }
}
