using System;

namespace NClient.Abstractions.Providers.Handling
{
    public class ClientHandlerSettings<TRequest, TResponse> : IClientHandlerSettings<TRequest, TResponse>
    {
        public Func<TRequest, TRequest> BeforeRequest { get; }
        public Func<TResponse, TResponse> AfterResponse { get; }
        
        public ClientHandlerSettings(Func<TRequest, TRequest> beforeRequest, Func<TResponse, TResponse> afterResponse)
        {
            BeforeRequest = beforeRequest;
            AfterResponse = afterResponse;
        }
    }
}
