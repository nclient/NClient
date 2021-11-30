using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Handling;

namespace NClient.Api.Tests.Stubs
{
    public class CustomHandler : IClientHandler<HttpRequestMessage, HttpResponseMessage>
    {
        public Task<HttpRequestMessage> HandleRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Request is starting...");
            return Task.FromResult(request);
        }
        
        public Task<HttpResponseMessage> HandleResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            Console.WriteLine("Request completed.");
            return Task.FromResult(response);
        }
    }
}
