using System;
using NClient.Providers.Transport;

namespace NClient.Providers.Validation
{
    /// <summary>The response validator settings for validation the contents of the response received from transport.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IResponseValidatorSettings<TRequest, TResponse>
    {
        /// <summary>The predicate for determining the success of the transport response.</summary>
        Predicate<IResponseContext<TRequest, TResponse>> IsSuccess { get; }
        
        /// <summary>The action that will be invoked if the response is unsuccessful.</summary>
        Action<IResponseContext<TRequest, TResponse>> OnFailure { get; }
    }
}
