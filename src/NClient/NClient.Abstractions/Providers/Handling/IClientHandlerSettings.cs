using System;

namespace NClient.Providers.Handling
{
    /// <summary>Settings for handling operations that handles the transport messages.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IClientHandlerSettings<TRequest, TResponse>
    {
        /// <summary>Gets the function that will be executed before a request.</summary>
        Func<TRequest, TRequest> BeforeRequest { get; }
        
        /// <summary>Gets the function that will be executed after a request.</summary>
        Func<TResponse, TResponse> AfterResponse { get; }
    }
}
