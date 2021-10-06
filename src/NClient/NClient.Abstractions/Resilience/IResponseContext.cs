using NClient.Abstractions.Invocation;

namespace NClient.Abstractions.Resilience
{
    public interface IResponseContext<TRequest, TResponse>
    {
        TRequest Request { get; }
        TResponse Response { get; }
        MethodInvocation MethodInvocation { get; }
    }
}
