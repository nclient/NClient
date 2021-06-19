using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;

namespace NClient.Abstractions.Handling
{
    public interface IClientHandler
    {
        Task<HttpRequest> HandleRequestAsync(
            HttpRequest httpRequest, MethodInvocation methodInvocation);
        Task<HttpResponse> HandleResponseAsync(
            HttpResponse httpResponse, MethodInvocation methodInvocation);
    }
}