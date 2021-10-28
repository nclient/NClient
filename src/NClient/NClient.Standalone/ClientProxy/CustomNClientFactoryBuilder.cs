using NClient.Common.Helpers;
using NClient.Standalone.ClientProxy.Building.Factory;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>
    /// The builder used to create the client factory with custom providers.
    /// </summary>
    public class CustomNClientFactoryBuilder : INClientFactoryBuilder
    {
        public INClientFactoryApiBuilder For(string factoryName)
        {
            Ensure.IsNotNullOrEmpty(factoryName, nameof(factoryName));
            return new NClientFactoryApiBuilder(factoryName);
        }
    }
}
