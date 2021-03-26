using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.AspNetProxy
{
    [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use INClientFactory.")]
    public interface INClientControllerFactory
    {
        TInterface Create<TInterface, TController>(string host)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface;
    }

    [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use NClientFactory.")]
    public class NClientControllerFactory : INClientControllerFactory
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IResiliencePolicyProvider _resiliencePolicyProvider;
        private readonly ILoggerFactory _loggerFactory;

        public NClientControllerFactory(
            IHttpClientProvider httpClientProvider, 
            IResiliencePolicyProvider resiliencePolicyProvider,
            ILoggerFactory loggerFactory)
        {
            _httpClientProvider = httpClientProvider;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _loggerFactory = loggerFactory;
        }

        public TInterface Create<TInterface, TController>(string host)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return new NClientControllerBuilder()
                .Use<TInterface, TController>(host, _httpClientProvider)
                .WithResiliencePolicy(_resiliencePolicyProvider)
                .WithLogging(_loggerFactory.CreateLogger<TInterface>())
                .Build();
        }
    }
}
