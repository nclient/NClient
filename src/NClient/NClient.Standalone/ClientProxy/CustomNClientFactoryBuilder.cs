using NClient.Abstractions;
using NClient.Abstractions.Builders;
using NClient.Common.Helpers;
using NClient.Standalone.ClientProxy.Building;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>
    /// The builder used to create the client factory with custom providers.
    /// </summary>
    public class CustomNClientFactoryBuilder : INClientFactoryBuilder
    {
        public INClientFactoryHttpClientBuilder For(string factoryName)
        {
            Ensure.IsNotNullOrEmpty(factoryName, nameof(factoryName));
            return new NClientFactoryHttpClientBuilder(factoryName);
        }
    }
}
