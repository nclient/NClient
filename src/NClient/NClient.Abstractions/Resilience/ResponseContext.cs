using NClient.Abstractions.Invocation;

namespace NClient.Abstractions.Resilience
{
    public class ResponseContext<TRequest, TResponse>
    {
        public TRequest Request { get; }
        public TResponse Response { get; }
        public MethodInvocation MethodInvocation { get; }

        public ResponseContext(TRequest request, TResponse response, MethodInvocation methodInvocation)
        {
            Request = request;
            Response = response;
            MethodInvocation = methodInvocation;
        }
    }
}
