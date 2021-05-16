using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Clients
{
    public interface IHttpNClient<T>
    {
        HttpResponse GetHttpResponse(Expression<Action<T>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        HttpResponse<TError> GetHttpResponse<TError>(Expression<Action<T>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        HttpValueResponse<TResult> GetHttpResponse<TResult>(Expression<Func<T, TResult>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        HttpValueResponse<TResult, TError> GetHttpResponse<TResult, TError>(Expression<Func<T, TResult>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        Task<HttpResponse> GetHttpResponse(Expression<Func<T, Task>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        Task<HttpResponse<TError>> GetHttpResponse<TError>(Expression<Func<T, Task>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        Task<HttpValueResponse<TResult>> GetHttpResponse<TResult>(Expression<Func<T, Task<TResult>>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        Task<HttpValueResponse<TResult, TError>> GetHttpResponse<TResult, TError>(Expression<Func<T, Task<TResult>>> apiMethodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
    }
}
