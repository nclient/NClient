using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.Providers.Handling;

namespace NClient.Sandbox.Client.ClientHandlers
{
    public class LoggingClientHandler : IClientHandler<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ILogger<LoggingClientHandler> _logger;

        public LoggingClientHandler(ILogger<LoggingClientHandler> logger)
        {
            _logger = logger;
        }

        public Task<HttpRequestMessage> HandleRequestAsync(HttpRequestMessage request)
        {
            return Task.FromResult(request);
        }

        public Task<HttpResponseMessage> HandleResponseAsync(HttpResponseMessage response)
        {
            _logger.LogDebug("The response with the body is received: {httpRequestContent}", response.Content);
            return Task.FromResult(response);
        }
    }
}
