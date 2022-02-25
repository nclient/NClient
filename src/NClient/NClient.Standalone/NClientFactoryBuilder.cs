using NClient.Common.Helpers;
using NClient.Standalone.ClientProxy.Building.Factory;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>The builder used to create the client factory with custom providers.</summary>
    public class NClientFactoryBuilder : INClientFactoryBuilder
    {
        /// <summary>Sets factory name. The factory name does not affect the functionality, it may be needed to identify the factory.</summary>
        /// <param name="factoryName">The factory name.</param>
        public INClientFactoryApiBuilder For(string factoryName)
        {
            Ensure.IsNotNullOrEmpty(factoryName, nameof(factoryName));
            return new NClientFactoryApiBuilder(factoryName);
        }
    }
}
