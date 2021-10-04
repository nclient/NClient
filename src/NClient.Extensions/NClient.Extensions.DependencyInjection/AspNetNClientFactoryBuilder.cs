using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Builders;
using NClient.Providers.HttpClient.System;

namespace NClient.Extensions.DependencyInjection
{
    // TODO: doc
    public class AspNetNClientFactoryBuilder
    {
        private readonly string? _httpClientName;
        private readonly IServiceProvider _serviceProvider;
        
        public AspNetNClientFactoryBuilder(string? httpClientName, IServiceProvider serviceProvider)
        {
            _httpClientName = httpClientName;
            _serviceProvider = serviceProvider;
        }
        
        public INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName)
        {
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            
            return new CustomNClientFactoryBuilder()
                .For(factoryName)
                .UsingSystemHttpClient(httpClientFactory, _httpClientName)
                .UsingJsonSerializer()
                .EnsuringSuccess()
                .WithoutHandling()
                .WithoutResilience()
                .WithLogging(loggerFactory);
        }
    }
}
