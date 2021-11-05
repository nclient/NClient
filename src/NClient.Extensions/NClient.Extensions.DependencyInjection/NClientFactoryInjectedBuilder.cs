using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Providers.Api.Rest.Extensions;

namespace NClient.Extensions.DependencyInjection
{
    // TODO: doc
    public interface INClientFactoryInjectedBuilder
    {
        INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName);
    }
    
    public class NClientFactoryInjectedBuilder : INClientFactoryInjectedBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string? _httpClientName;
        
        public NClientFactoryInjectedBuilder(IServiceProvider serviceProvider, string? httpClientName = null)
        {
            _serviceProvider = serviceProvider;
            _httpClientName = httpClientName;
        }
        
        public INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName)
        {
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            
            return new NClientFactoryBuilder()
                .For(factoryName)
                .UsingRestApi()
                .UsingSystemHttpTransport(httpClientFactory, _httpClientName)
                .UsingJsonSerializer()
                .WithAdvancedResponseValidation(x => x
                    .ForTransport().UseSystemResponseValidation())
                .WithoutHandling()
                .WithoutResilience()
                .WithAdvancedResults(x => x
                    .ForTransport().UseHttpResults()
                    .ForClient().UseResults())
                .WithLogging(loggerFactory);
        }
    }
}
