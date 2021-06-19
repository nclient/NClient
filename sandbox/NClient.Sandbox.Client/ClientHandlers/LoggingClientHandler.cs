using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;

namespace NClient.Sandbox.Client.ClientHandlers
{
    public class LoggingClientHandler : IClientHandler
    {
        private readonly ILogger<LoggingClientHandler> _logger;

        public LoggingClientHandler(ILogger<LoggingClientHandler> logger)
        {
            _logger = logger;
        }

        public Task<HttpRequest> HandleRequestAsync(HttpRequest httpRequest, MethodInvocation methodInvocation)
        {
            return Task.FromResult(httpRequest);
        }

        public Task<HttpResponse> HandleResponseAsync(HttpResponse httpResponse, MethodInvocation methodInvocation)
        {
            _logger.LogDebug("The response (id: {httpRequestId}) with the body is received: {httpRequestContent}", httpResponse.Request.Id, httpResponse.Content);
            return Task.FromResult(httpResponse);
        }
    }
}