using NClient.Abstractions.Invocation;

namespace NClient.Abstractions.Resilience
{
    public class ResponseContext<TResponse>
    {
        public TResponse Response { get; }
        public MethodInvocation MethodInvocation { get; }

        public ResponseContext(TResponse response, MethodInvocation methodInvocation)
        {
            Response = response;
            MethodInvocation = methodInvocation;
        }
    }
}
