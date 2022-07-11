using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NClient.Common.Helpers;

namespace NClient
{
    /// <summary>The client builder factory for a REST-like web API with JSON-formatted data.</summary>
    public interface IRestNClientFactoryBuilder
    {
        /// <summary>Sets factory name. The factory name does not affect the functionality, it may be needed to identify the factory.</summary>
        /// <param name="factoryName">The factory name.</param>
        INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName);
    }
    
    /// <summary>The client builder factory for a REST-like web API with JSON-formatted data.</summary>
    public class RestNClientFactoryBuilder : IRestNClientFactoryBuilder
    {
        private readonly string? _internalFactoryName;
        private readonly IServiceProvider? _serviceProvider;

        public RestNClientFactoryBuilder()
        {
        }
        
        internal RestNClientFactoryBuilder(string internalFactoryName, IServiceProvider serviceProvider)
        {
            _internalFactoryName = internalFactoryName;
            _serviceProvider = serviceProvider;
        }
        
        /// <summary>Sets factory name. The factory name does not affect the functionality, it may be needed to identify the factory.</summary>
        /// <param name="factoryName">The factory name.</param>
        public INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName)
        {
            Ensure.IsNotNullOrEmpty(factoryName, nameof(factoryName));
            
            var optionsMonitor = _serviceProvider?.GetService<IOptionsMonitorCache<NClientFactoryBuilderOptions<HttpRequestMessage, HttpResponseMessage>>>();
            var defaultOptions = _serviceProvider?.GetService<IOptions<NClientFactoryBuilderOptions<HttpRequestMessage, HttpResponseMessage>>>()?.Value
                ?? new NClientFactoryBuilderOptions<HttpRequestMessage, HttpResponseMessage>();
            var builderOptions = optionsMonitor?.GetOrAdd(_internalFactoryName, createOptions: () => defaultOptions);
            var httpClientFactory = _serviceProvider?.GetService<IHttpClientFactory>();
            var jsonSerializerOptions = _serviceProvider?.GetService<IOptions<JsonSerializerOptions>>()?.Value;
            var loggerFactory = _serviceProvider?.GetService<ILoggerFactory>();
            
            var transportBuilder = new NClientFactoryBuilder()
                .For(factoryName)
                .UsingRestApi();
            var serializationBuilder = httpClientFactory is null
                ? transportBuilder.UsingSystemNetHttpTransport()
                : transportBuilder.UsingSystemNetHttpTransport(httpClientFactory, _internalFactoryName);
            var optionalBuilder = jsonSerializerOptions is null
                ? serializationBuilder.UsingSystemTextJsonSerialization()
                : serializationBuilder.UsingSystemTextJsonSerialization(jsonSerializerOptions);
            optionalBuilder = loggerFactory is null
                ? optionalBuilder.WithoutLogging()
                : optionalBuilder.WithLogging(loggerFactory);
            
            optionalBuilder = optionalBuilder    
                .WithResponseValidation()
                .WithResponseToHttpResponseMapping()
                .WithResponseToStreamMapping()
                .WithResponseToResultMapping();

            if (builderOptions is null)
                return optionalBuilder;
            
            var finalBuilder = optionalBuilder;
            foreach (var builderAction in builderOptions.BuilderActions)
            {
                finalBuilder = builderAction.Invoke(optionalBuilder);
            }
            
            return finalBuilder;
        }
    }
}
