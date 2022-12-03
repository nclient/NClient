using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NClient.Common.Helpers;
using NClient.Providers.Host;
using NClient.Providers.Transport.SystemNetHttp.Mapping;
using NClient.Standalone.Client.Host;

namespace NClient
{
    /// <summary>The client builder for a REST-like web API with JSON-formatted data.</summary>
    public interface IRestNClientBuilder
    {
        /// <summary>Sets client interface type and web service host.</summary>
        /// <param name="host">The address of the web service host.</param>
        /// <typeparam name="TClient">The type of interface of controller used to create the client.</typeparam>
        INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class;
        
        /// <summary>Sets client interface type and web service host.</summary>
        /// <param name="host">The address of the web service host.</param>
        /// <typeparam name="TClient">The type of interface of controller used to create the client.</typeparam>
        INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(Uri host) 
            where TClient : class;
        
        /// <summary>Sets client interface type and web service host.</summary>
        /// <param name="host">The object with address of the web service host.</param>
        /// <typeparam name="TClient">The type of interface of controller used to create the client.</typeparam>
        INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(IHost host) 
            where TClient : class;
    }
    
    /// <summary>The client builder for a REST-like web API with JSON-formatted data.</summary>
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

        /// <summary>Sets client interface type and web service host.</summary>
        /// <param name="host">The address of the web service host.</param>
        /// <typeparam name="TClient">The type of interface of controller used to create the client.</typeparam>
        public INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host)
            where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));

            return For<TClient>(new Uri(host));
        }

        /// <summary>Sets client interface type and web service host.</summary>
        /// <param name="host">The address of the web service host.</param>
        /// <typeparam name="TClient">The type of interface of controller used to create the client.</typeparam>
        public INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(Uri host)
            where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));

            return For<TClient>(new Host(host));
        }

        /// <summary>Sets client interface type and web service host.</summary>
        /// <param name="host">The address of the web service host.</param>
        /// <typeparam name="TClient">The type of interface of controller used to create the client.</typeparam>
        public INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(IHost host) 
            where TClient : class
        {
            var optionsMonitor = _serviceProvider?.GetService<IOptionsMonitorCache<NClientBuilderOptions<TClient, HttpRequestMessage, HttpResponseMessage>>>();
            var defaultOptions = _serviceProvider?.GetService<IOptions<NClientBuilderOptions<TClient, HttpRequestMessage, HttpResponseMessage>>>()?.Value
                ?? new NClientBuilderOptions<TClient, HttpRequestMessage, HttpResponseMessage>();
            var builderOptions = optionsMonitor?.GetOrAdd(_internalClientName, createOptions: () => defaultOptions);
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
                .WithResponseToStreamMapping()
                .WithResponseToResultMapping()
                .WithResponseMapping(new StreamContentResponseMapper())
                .WithResponseMapping(new HttpFileContentResponseMapper());

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
