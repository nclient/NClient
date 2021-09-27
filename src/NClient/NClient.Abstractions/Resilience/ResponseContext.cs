using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;

namespace NClient.Abstractions.Resilience
{
    public class ResponseContext
    {
        public HttpResponse HttpResponse { get; }
        public MethodInvocation MethodInvocation { get; }

        public ResponseContext(HttpResponse httpResponse, MethodInvocation methodInvocation)
        {
            HttpResponse = httpResponse;
            MethodInvocation = methodInvocation;
        }
    }
}
