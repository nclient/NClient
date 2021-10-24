using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NClient.Extensions.DependencyInjection
{
    // TODO: doc
    public interface IInjectedFactoryBuilder
    {
        INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName);
    }
    
    public class InjectedFactoryBuilder : IInjectedFactoryBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string? _httpClientName;
        
        public InjectedFactoryBuilder(IServiceProvider serviceProvider, string? httpClientName = null)
        {
            _serviceProvider = serviceProvider;
            _httpClientName = httpClientName;
        }
        
        public INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName)
        {
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            
            return new CustomNClientFactoryBuilder()
                .For(factoryName)
                .UsingSystemHttpTransport(httpClientFactory, _httpClientName)
                .UsingJsonSerializer()
                .WithResponseValidation()
                .WithoutHandling()
                .WithoutResilience()
                .WithLogging(loggerFactory);
        }
    }
}
