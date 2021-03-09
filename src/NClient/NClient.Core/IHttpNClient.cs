using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NClient.Providers.HttpClient;
using NClient.Providers.Resilience.Abstractions;

namespace NClient.Core
{
    public interface IHttpNClient<T> where T : INClient
    {
        HttpResponse GetHttpResponse(Expression<Action<T>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        HttpResponse<TResult> GetHttpResponse<TResult>(Expression<Func<T, TResult>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        Task<HttpResponse> GetHttpResponse(Expression<Func<T, Task>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        Task<HttpResponse<TResult>> GetHttpResponse<TResult>(Expression<Func<T, Task<TResult>>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
    }
}
