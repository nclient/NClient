using System;
using System.Net;

namespace NClient.Abstractions.Exceptions.Factories
{
    internal static class OuterExceptionFactory
    {
        public static HttpRequestNClientException HttpRequestFailed(
            HttpStatusCode statusCode, string? errorMessage, string? content, Exception innerException) =>
            new(statusCode, errorMessage, content, innerException);
    }
}
