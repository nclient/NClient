using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NClient.Common.Helpers;

namespace NClient
{
    public interface IRestNClientBuilder
    {
        INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(Uri host) 
            where TClient : class;
    }
    
    /// <summary>
    /// The builder used to create the client.
    /// </summary>
    public class RestNClientBuilder : IRestNClientBuilder
    {
        private readonly string? _internalClientName;
        private readonly IServiceProvider? _serviceProvider;

        public RestNClientBuilder()
        {
        }
        
        public RestNClientBuilder(string internalClientName, IServiceProvider serviceProvider)
        {
            _internalClientName = internalClientName;
            _serviceProvider = serviceProvider;
        }

        public INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(Uri host) 
            where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));
            
            var optionsMonitor = _serviceProvider?.GetService<IOptionsMonitor<NClientBuilderOptions<TClient, HttpRequestMessage, HttpResponseMessage>>>();
            var builderOptions = optionsMonitor?.Get(_internalClientName);
            var httpClientFactory = _serviceProvider?.GetService<IHttpClientFactory>();
            var jsonSerializerOptions = _serviceProvider?.GetService<IOptions<JsonSerializerOptions>>()?.Value;
            var loggerFactory = _serviceProvider?.GetService<ILoggerFactory>();

            var transportBuilder = new NClientBuilder()
                .For<TClient>(host)
                .UsingRestApi();
            var serializationBuilder = httpClientFactory is null
                ? transportBuilder.UsingSystemNetHttpTransport()
                : transportBuilder.UsingSystemNetHttpTransport(httpClientFactory, _internalClientName);
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

            var finalBuilder = optionalBuilder;
            foreach (var builderAction in builderOptions.BuilderActions)
            {
                finalBuilder = builderAction.Invoke(optionalBuilder);
            }
            
            return finalBuilder;
        }
    }
}
