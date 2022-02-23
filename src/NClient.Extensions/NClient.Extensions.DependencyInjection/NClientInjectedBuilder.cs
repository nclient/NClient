using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NClient.Extensions.DependencyInjection
{
    // TODO: doc
    public interface INClientInjectedBuilder
    {
        INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class;
    }
    
    public class NClientInjectedBuilder : INClientInjectedBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string? _httpClientName;
        
        public NClientInjectedBuilder(IServiceProvider serviceProvider, string? httpClientName = null)
        {
            _serviceProvider = serviceProvider;
            _httpClientName = httpClientName;
        }
        
        public INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class
        {
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            
            return new NClientBuilder()
                .For<TClient>(host)
                .UsingRestApi()
                .UsingSystemNetHttpTransport(httpClientFactory, _httpClientName)
                .UsingJsonSerializer()
                .WithResponseValidation()
                .WithoutHandling()
                .WithoutResilience()
                .WithResponseToHttpResponseMapping()
                .WithResponseToResultMapping()
                .WithLogging(loggerFactory);
        }
    }
}
