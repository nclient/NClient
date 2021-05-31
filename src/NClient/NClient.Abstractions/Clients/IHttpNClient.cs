using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Clients
{
    public interface IHttpNClient<T>
    {
        HttpResponse GetHttpResponse(Expression<Action<T>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        HttpResponseWithError<TError> GetHttpResponse<TError>(Expression<Action<T>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        HttpResponse<TResult> GetHttpResponse<TResult>(Expression<Func<T, TResult>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        HttpResponseWithError<TResult, TError> GetHttpResponse<TResult, TError>(Expression<Func<T, TResult>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        Task<HttpResponse> GetHttpResponse(Expression<Func<T, Task>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        Task<HttpResponseWithError<TError>> GetHttpResponse<TError>(Expression<Func<T, Task>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        Task<HttpResponse<TResult>> GetHttpResponse<TResult>(Expression<Func<T, Task<TResult>>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        Task<HttpResponseWithError<TResult, TError>> GetHttpResponse<TResult, TError>(Expression<Func<T, Task<TResult>>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
    }
}
