using System;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.Standalone
{
    public interface INClientFactory
    {
        T Create<T>(string host) where T : class;

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Create<T> method.")]
        TInterface Create<TInterface, TController>(string host)
            where TInterface : class
            where TController : TInterface;
    }

    public class NClientFactory : INClientFactory
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IResiliencePolicyProvider _resiliencePolicyProvider;
        private readonly ILoggerFactory _loggerFactory;

        public NClientFactory(
            IHttpClientProvider httpClientProvider, 
            IResiliencePolicyProvider resiliencePolicyProvider,
            ILoggerFactory loggerFactory)
        {
            _httpClientProvider = httpClientProvider;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _loggerFactory = loggerFactory;
        }

        public T Create<T>(string host) where T : class
        {
            return new NClientBuilder()
                .Use<T>(host, _httpClientProvider)
                .WithResiliencePolicy(_resiliencePolicyProvider)
                .WithLogging(_loggerFactory.CreateLogger<T>())
                .Build();
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Create<T> method.")]
        public TInterface Create<TInterface, TController>(string host)
            where TInterface : class
            where TController : TInterface
        {
            return new NClientBuilder()
                .Use<TInterface, TController>(host, _httpClientProvider)
                .WithResiliencePolicy(_resiliencePolicyProvider)
                .WithLogging(_loggerFactory.CreateLogger<TInterface>())
                .Build();
        }
    }
}
