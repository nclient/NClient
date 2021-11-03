using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Providers.Api.Rest.Extensions;

namespace NClient.Extensions.DependencyInjection
{
    // TODO: doc
    public interface INClientInjectedFactoryBuilder
    {
        INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName);
    }
    
    public class NClientInjectedFactoryBuilder : INClientInjectedFactoryBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string? _httpClientName;
        
        public NClientInjectedFactoryBuilder(IServiceProvider serviceProvider, string? httpClientName = null)
        {
            _serviceProvider = serviceProvider;
            _httpClientName = httpClientName;
        }
        
        public INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName)
        {
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            
            return new NClientAdvancedFactoryBuilder()
                .For(factoryName)
                .UsingRestApi()
                .UsingSystemHttpTransport(httpClientFactory, _httpClientName)
                .UsingJsonSerializer()
                .WithResponseValidation()
                .WithoutHandling()
                .WithoutResilience()
                .WithResults()
                .WithHttpResults()
                .WithLogging(loggerFactory);
        }
    }
}
