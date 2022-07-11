﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>An abstraction that allows the client to get transport response.</summary>
    /// <typeparam name="TClient">The client interface type.</typeparam>
    public interface ITransportNClient<TClient>
    {
        /// <summary>Returns transport response of the server. </summary>
        /// <param name="methodCall">The client method call.</param>
        IResponse GetTransportResponse(Expression<Action<TClient>> methodCall);

        /// <summary>Returns response of the server. </summary>
        /// <param name="methodCall">The client method call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TRequest">The type of transport request that is used in the transport implementation.</typeparam>
        /// <typeparam name="TResponse">The type of transport response that is used in the transport implementation.</typeparam>
        IResponse GetTransportResponse<TRequest, TResponse>(Expression<Action<TClient>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
        
        /// <summary>Returns response of the server. </summary>
        /// <param name="methodCall">The client method to call.</param>
        IResponseWithError<TError> GetTransportResponse<TError>(Expression<Action<TClient>> methodCall);

        /// <summary>Returns response of the server. </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed status code.</typeparam>
        /// <typeparam name="TRequest">The type of transport request that is used in the transport implementation.</typeparam>
        /// <typeparam name="TResponse">The type of transport response that is used in the transport implementation.</typeparam>
        IResponseWithError<TError> GetTransportResponse<TError, TRequest, TResponse>(Expression<Action<TClient>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>Returns response of the server. </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        IResponse<TResult> GetTransportResponse<TResult>(Expression<Func<TClient, TResult>> methodCall);

        /// <summary>Returns response of the server. </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TRequest">The type of transport request that is used in the transport implementation.</typeparam>
        /// <typeparam name="TResponse">The type of transport response that is used in the transport implementation.</typeparam>
        IResponse<TResult> GetTransportResponse<TResult, TRequest, TResponse>(Expression<Func<TClient, TResult>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
        
        /// <summary>Returns response of the server. </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed status code.</typeparam>
        IResponseWithError<TResult, TError> GetTransportResponse<TResult, TError>(Expression<Func<TClient, TResult>> methodCall);

        /// <summary>Returns response of the server. </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed status code.</typeparam>
        /// <typeparam name="TResponse">The type of transport response that is used in the transport implementation.</typeparam>
        IResponseWithError<TResult, TError> GetTransportResponse<TResult, TError, TRequest, TResponse>(Expression<Func<TClient, TResult>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>Returns response of the server. </summary>
        /// <param name="methodCall">The client method to call.</param>
        Task<IResponse> GetTransportResponse(Expression<Func<TClient, Task>> methodCall);
        
        /// <summary>Returns response of the server. </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TRequest">The type of transport request that is used in the transport implementation.</typeparam>
        /// <typeparam name="TResponse">The type of transport response that is used in the transport implementation.</typeparam>
        Task<IResponse> GetTransportResponse<TRequest, TResponse>(Expression<Func<TClient, Task>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>Returns response of the server.</summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed status code.</typeparam>
        Task<IResponseWithError<TError>> GetTransportResponse<TError>(Expression<Func<TClient, Task>> methodCall);

        /// <summary>Returns response of the server.</summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed status code.</typeparam>
        /// <typeparam name="TRequest">The type of transport request that is used in the transport implementation.</typeparam>
        /// <typeparam name="TResponse">The type of transport response that is used in the transport implementation.</typeparam>
        Task<IResponseWithError<TError>> GetTransportResponse<TError, TRequest, TResponse>(Expression<Func<TClient, Task>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>Returns response of the server.</summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        Task<IResponse<TResult>> GetTransportResponse<TResult>(Expression<Func<TClient, Task<TResult>>> methodCall);

        /// <summary>Returns response of the server.</summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TRequest">The type of transport request that is used in the transport implementation.</typeparam>
        /// <typeparam name="TResponse">The type of transport response that is used in the transport implementation.</typeparam>
        Task<IResponse<TResult>> GetTransportResponse<TResult, TRequest, TResponse>(Expression<Func<TClient, Task<TResult>>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>Returns response of the server.</summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed status code.</typeparam>
        Task<IResponseWithError<TResult, TError>> GetTransportResponse<TResult, TError>(Expression<Func<TClient, Task<TResult>>> methodCall);
        
        /// <summary>Returns response of the server.</summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TError">The type to deserialize the response content used when returning a failed status code.</typeparam>
        /// <typeparam name="TResponse">The type of transport response that is used in the transport implementation.</typeparam>
        Task<IResponseWithError<TResult, TError>> GetTransportResponse<TResult, TError, TRequest, TResponse>(Expression<Func<TClient, Task<TResult>>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
    }
}
