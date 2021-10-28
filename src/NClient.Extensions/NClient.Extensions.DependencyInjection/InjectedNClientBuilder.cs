using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Providers.Api.Rest.Extensions;

namespace NClient.Extensions.DependencyInjection
{
    // TODO: doc
    public interface IInjectedNClientBuilder
    {
        INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class;
    }
    
    public class InjectedNClientBuilder : IInjectedNClientBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string? _httpClientName;
        
        public InjectedNClientBuilder(IServiceProvider serviceProvider, string? httpClientName = null)
        {
            _serviceProvider = serviceProvider;
            _httpClientName = httpClientName;
        }
        
        public INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class
        {
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            
            return new CustomNClientBuilder()
                .For<TClient>(host)
                .UsingRestApi()
                .UsingSystemHttpTransport(httpClientFactory, _httpClientName)
                .UsingJsonSerializer()
                .WithResponseValidation()
                .WithoutHandling()
                .WithoutResilience()
                .WithLogging(loggerFactory);
        }
    }
}
