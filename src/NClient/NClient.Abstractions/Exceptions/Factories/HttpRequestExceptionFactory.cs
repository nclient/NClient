using System;
using System.Net;
using NClient.Abstractions.HttpClients;

namespace NClient.Abstractions.Exceptions.Factories
{
    internal static class HttpRequestExceptionFactory
    {
        public static ClientHttpRequestException HttpRequestFailed(HttpResponse httpResponse) =>
            new(httpResponse);
    }
}
