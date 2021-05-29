using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.OptionalNClientBuilders;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Serialization.System;

namespace NClient
{
    public class NClientFactoryBuilder : NClientStandaloneFactoryBuilder
    {
        public NClientFactoryBuilder() : base(new SystemHttpClientProvider(), new SystemSerializerProvider())
        {
        }
    }
}