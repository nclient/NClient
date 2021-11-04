using System.Net.Http;
using NClient.Providers.Results.HttpResults;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseHttpResultsExtensions
    {
        public static INClientResultsSelector<HttpRequestMessage, HttpResponseMessage> UseHttpResults(
            this INClientTransportResultsSetter<HttpRequestMessage, HttpResponseMessage> clientOptionalBuilder)
        {
            return clientOptionalBuilder.Use(new HttpResponseBuilderProvider());
        }
    }
}
