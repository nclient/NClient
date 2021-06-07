using NClient.Abstractions.HttpClients;

namespace NClient.Abstractions.Exceptions.Factories
{
    internal interface IClientHttpRequestExceptionFactory
    {
        ClientHttpRequestException HttpRequestFailed(HttpResponse httpResponse);
    }

    internal class ClientHttpRequestExceptionFactory : IClientHttpRequestExceptionFactory
    {
        public ClientHttpRequestException HttpRequestFailed(HttpResponse httpResponse) =>
            new(httpResponse);
    }
}
