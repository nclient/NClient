using System;

namespace NClient.Providers.Handling
{
    public interface IClientHandlerSettings<TRequest, TResponse>
    {
        Func<TRequest, TRequest> BeforeRequest { get; }
        Func<TResponse, TResponse> AfterResponse { get; }
    }
}
