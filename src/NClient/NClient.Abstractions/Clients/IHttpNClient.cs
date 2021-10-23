using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NClient.Abstractions.Providers.Resilience;
using NClient.Abstractions.Providers.Transport;

namespace NClient.Abstractions.Clients
{
    // TODO: doc
    public interface IHttpNClient<T>
    {
        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method call.</param>
        IHttpResponse GetHttpResponse(Expression<Action<T>> methodCall);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        IHttpResponse GetHttpResponse<TRequest, TResponse>(Expression<Action<T>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
        
        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        IHttpResponseWithError<TError> GetHttpResponse<TError>(Expression<Action<T>> methodCall);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        IHttpResponseWithError<TError> GetHttpResponse<TError, TRequest, TResponse>(Expression<Action<T>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        IHttpResponse<TResult> GetHttpResponse<TResult>(Expression<Func<T, TResult>> methodCall);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        IHttpResponse<TResult> GetHttpResponse<TResult, TRequest, TResponse>(Expression<Func<T, TResult>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
        
        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        IHttpResponseWithError<TResult, TError> GetHttpResponse<TResult, TError>(Expression<Func<T, TResult>> methodCall);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        IHttpResponseWithError<TResult, TError> GetHttpResponse<TResult, TError, TRequest, TResponse>(Expression<Func<T, TResult>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        Task<IHttpResponse> GetHttpResponse(Expression<Func<T, Task>> methodCall);
        
        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        Task<IHttpResponse> GetHttpResponse<TRequest, TResponse>(Expression<Func<T, Task>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        Task<IHttpResponseWithError<TError>> GetHttpResponse<TError>(Expression<Func<T, Task>> methodCall);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        Task<IHttpResponseWithError<TError>> GetHttpResponse<TError, TRequest, TResponse>(Expression<Func<T, Task>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        Task<IHttpResponse<TResult>> GetHttpResponse<TResult>(Expression<Func<T, Task<TResult>>> methodCall);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        Task<IHttpResponse<TResult>> GetHttpResponse<TResult, TRequest, TResponse>(Expression<Func<T, Task<TResult>>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        Task<IHttpResponseWithError<TResult, TError>> GetHttpResponse<TResult, TError>(Expression<Func<T, Task<TResult>>> methodCall);
        
        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        Task<IHttpResponseWithError<TResult, TError>> GetHttpResponse<TResult, TError, TRequest, TResponse>(Expression<Func<T, Task<TResult>>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
    }
}
