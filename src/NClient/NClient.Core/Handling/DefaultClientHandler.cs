using System.Threading.Tasks;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;

namespace NClient.Core.Handling
{
    public class DefaultClientHandler : IClientHandler
    {
        public virtual Task<HttpRequest> HandleRequestAsync(
            HttpRequest httpRequest, MethodInvocation methodInvocation)
        {
            return Task.FromResult(httpRequest);
        }

        public virtual Task<HttpResponse> HandleResponseAsync(
            HttpResponse httpResponse, MethodInvocation methodInvocation)
        {
            if (typeof(HttpResponse).IsAssignableFrom(methodInvocation.ResultType))
                return Task.FromResult(httpResponse);
            return Task.FromResult(httpResponse.EnsureSuccess());
        }
    }
}