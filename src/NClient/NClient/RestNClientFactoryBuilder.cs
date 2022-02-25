using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NClient
{
    public interface IRestNClientFactoryBuilder
    {
        INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName);
    }
    
    /// <summary>
    /// The builder used to create the client factory.
    /// </summary>
    public class RestNClientFactoryBuilder : IRestNClientFactoryBuilder
    {
        private readonly IServiceProvider? _serviceProvider;

        public RestNClientFactoryBuilder()
        {
        }
        
        public RestNClientFactoryBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName)
        {
            var optionsMonitor = _serviceProvider?.GetService<IOptionsMonitor<NClientFactoryBuilderOptions<HttpRequestMessage, HttpResponseMessage>>>();
            var builderOptions = optionsMonitor?.Get(factoryName);
            var httpClientFactory = _serviceProvider?.GetService<IHttpClientFactory>();
            var jsonSerializerOptions = _serviceProvider?.GetService<IOptions<JsonSerializerOptions>>()?.Value;
            var loggerFactory = _serviceProvider?.GetService<ILoggerFactory>();
            
            var transportBuilder = new NClientFactoryBuilder()
                .For(factoryName)
                .UsingRestApi();
            var serializationBuilder = httpClientFactory is null
                ? transportBuilder.UsingSystemNetHttpTransport()
                : transportBuilder.UsingSystemNetHttpTransport(httpClientFactory, factoryName);
            var optionalBuilder = jsonSerializerOptions is null
                ? serializationBuilder.UsingSystemTextJsonSerialization()
                : serializationBuilder.UsingSystemTextJsonSerialization(jsonSerializerOptions);
            optionalBuilder = loggerFactory is null
                ? optionalBuilder.WithoutLogging()
                : optionalBuilder.WithLogging(loggerFactory);
            
            optionalBuilder = optionalBuilder    
                .WithResponseValidation()
                .WithResponseToHttpResponseMapping()
                .WithResponseToResultMapping();

            if (builderOptions is null)
                return optionalBuilder;
            
            foreach (var builderAction in builderOptions.BuilderActions)
            {
                builderAction.Invoke(optionalBuilder);
            }
            
            return optionalBuilder;
        }
    }
}
