using NClient.Abstractions.Invocation;

namespace NClient.Abstractions.Resilience
{
    public class ResponseContext<TRequest, TResponse> : IResponseContext<TRequest, TResponse>
    {
        public TRequest Request { get; }
        public TResponse Response { get; }
        public IMethodInvocation MethodInvocation { get; }

        public ResponseContext(TRequest request, TResponse response, IMethodInvocation methodInvocation)
        {
            Request = request;
            Response = response;
            MethodInvocation = methodInvocation;
        }
    }
}
