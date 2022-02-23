using System;

namespace NClient.Providers.Handling
{
    /// <summary>Settings for handling operations that handles the transport messages.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public class ClientHandlerSettings<TRequest, TResponse> : IClientHandlerSettings<TRequest, TResponse>
    {
        /// <summary>Gets the function that will be executed before a request.</summary>
        public Func<TRequest, TRequest> BeforeRequest { get; }
        
        /// <summary>Gets the function that will be executed after a request.</summary>
        public Func<TResponse, TResponse> AfterResponse { get; }
        
        /// <summary>Initializes settings for handling operations that handles the transport messages.</summary>
        /// <param name="beforeRequest">The function that will be executed before a request.</param>
        /// <param name="afterResponse">The function that will be executed after a request.</param>
        public ClientHandlerSettings(Func<TRequest, TRequest> beforeRequest, Func<TResponse, TResponse> afterResponse)
        {
            BeforeRequest = beforeRequest;
            AfterResponse = afterResponse;
        }
    }
}
