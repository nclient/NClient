using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Clients
{
    public interface IHttpNClient<T>
    {
        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method call.</param>
        HttpResponse GetHttpResponse(Expression<Action<T>> methodCall);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        HttpResponse GetHttpResponse<TRequest, TResponse>(Expression<Action<T>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
        
        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        HttpResponseWithError<TError> GetHttpResponse<TError>(Expression<Action<T>> methodCall);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        HttpResponseWithError<TError> GetHttpResponse<TError, TRequest, TResponse>(Expression<Action<T>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        HttpResponse<TResult> GetHttpResponse<TResult>(Expression<Func<T, TResult>> methodCall);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        HttpResponse<TResult> GetHttpResponse<TResult, TRequest, TResponse>(Expression<Func<T, TResult>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
        
        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        HttpResponseWithError<TResult, TError> GetHttpResponse<TResult, TError>(Expression<Func<T, TResult>> methodCall);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        HttpResponseWithError<TResult, TError> GetHttpResponse<TResult, TError, TRequest, TResponse>(Expression<Func<T, TResult>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        Task<HttpResponse> GetHttpResponse(Expression<Func<T, Task>> methodCall);
        
        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        Task<HttpResponse> GetHttpResponse<TRequest, TResponse>(Expression<Func<T, Task>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        Task<HttpResponseWithError<TError>> GetHttpResponse<TError>(Expression<Func<T, Task>> methodCall);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        Task<HttpResponseWithError<TError>> GetHttpResponse<TError, TRequest, TResponse>(Expression<Func<T, Task>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        Task<HttpResponse<TResult>> GetHttpResponse<TResult>(Expression<Func<T, Task<TResult>>> methodCall);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        Task<HttpResponse<TResult>> GetHttpResponse<TResult, TRequest, TResponse>(Expression<Func<T, Task<TResult>>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        Task<HttpResponseWithError<TResult, TError>> GetHttpResponse<TResult, TError>(Expression<Func<T, Task<TResult>>> methodCall);
        
        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        Task<HttpResponseWithError<TResult, TError>> GetHttpResponse<TResult, TError, TRequest, TResponse>(Expression<Func<T, Task<TResult>>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
    }
}
