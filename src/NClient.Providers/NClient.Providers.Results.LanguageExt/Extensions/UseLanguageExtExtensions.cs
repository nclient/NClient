using System.Net.Http;
using NClient.Providers.Results.LanguageExt;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseHttpResultsExtensions
    {
        public static INClientResultsSelector<HttpRequestMessage, HttpResponseMessage> UseLanguageExt(
            this INClientResultsSetter<HttpRequestMessage, HttpResponseMessage> clientOptionalBuilder)
        {
            return clientOptionalBuilder.Use(new EitherBuilderProvider());
        }
        
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> UseLanguageExt(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResults(new EitherBuilderProvider());
        }
    }
}
