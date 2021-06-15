﻿using System;
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
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        HttpResponse GetHttpResponse(Expression<Action<T>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        HttpResponseWithError<TError> GetHttpResponse<TError>(Expression<Action<T>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        HttpResponse<TResult> GetHttpResponse<TResult>(Expression<Func<T, TResult>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        HttpResponseWithError<TResult, TError> GetHttpResponse<TResult, TError>(Expression<Func<T, TResult>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        Task<HttpResponse> GetHttpResponse(Expression<Func<T, Task>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        Task<HttpResponseWithError<TError>> GetHttpResponse<TError>(Expression<Func<T, Task>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        Task<HttpResponse<TResult>> GetHttpResponse<TResult>(Expression<Func<T, Task<TResult>>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);

        /// <summary>
        /// Returns HTTP response of the server. 
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed HTTP status code.</typeparam>
        Task<HttpResponseWithError<TResult, TError>> GetHttpResponse<TResult, TError>(Expression<Func<T, Task<TResult>>> methodCall, IResiliencePolicyProvider? resiliencePolicyProvider = null);
    }
}
