using System;
using System.Linq.Expressions;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Clients
{
    public interface IResilienceNClient<T>
    {
        /// <summary>
        /// Makes a client method call with a specific resilience policy provider.
        /// </summary>
        /// <param name="methodCall">The client method to call.</param>
        /// <param name="resiliencePolicyProvider">The specific resilience policy provider for calling the method.</param>
        /// <typeparam name="TResult">The type to deserialize the response content.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the HTTP client implementation.</typeparam>
        TResult Invoke<TResult, TRequest, TResponse>(Expression<Func<T, TResult>> methodCall, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
    }
}
