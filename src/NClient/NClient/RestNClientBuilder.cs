using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NClient.Providers.Api.Rest.Extensions;

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
        private readonly IServiceProvider? _serviceProvider;
        private readonly string? _clientName;

        public RestNClientBuilder()
        {
        }
        
        public RestNClientBuilder(IServiceProvider serviceProvider, string clientName)
        {
            _serviceProvider = serviceProvider;
            _clientName = clientName;
        }

        public INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(Uri host) 
            where TClient : class
        {
            var optionsMonitor = _serviceProvider?.GetService<IOptionsMonitor<NClientBuilderOptions<TClient, HttpRequestMessage, HttpResponseMessage>>>();
            var builderOptions = optionsMonitor?.Get(_clientName);
            var httpClientFactory = _serviceProvider?.GetService<IHttpClientFactory>();
            var jsonSerializerOptions = _serviceProvider?.GetService<IOptions<JsonSerializerOptions>>()?.Value;
            var loggerFactory = _serviceProvider?.GetService<ILoggerFactory>();
            
            var transportBuilder = new NClientBuilder()
                .For<TClient>(host)
                .UsingRestApi();
            var serializationBuilder = httpClientFactory is null
                ? transportBuilder.UsingSystemNetHttpTransport()
                : transportBuilder.UsingSystemNetHttpTransport(httpClientFactory, _clientName);
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
