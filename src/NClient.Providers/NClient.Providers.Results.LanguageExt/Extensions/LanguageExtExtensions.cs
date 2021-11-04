using System.Net.Http;
using NClient.Providers.Results.LanguageExt;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class LanguageExtExtensions
    {
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithLanguageExtResults<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> clientOptionalBuilder) 
            where TClient : class
        {
            return clientOptionalBuilder.AsAdvanced()
                .WithResults(x => x
                    .ForClient().Use(new EitherBuilderProvider()))
                .AsBasic();
        }
        
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithLanguageExtResults(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResults(new EitherBuilderProvider());
        }
    }
}
