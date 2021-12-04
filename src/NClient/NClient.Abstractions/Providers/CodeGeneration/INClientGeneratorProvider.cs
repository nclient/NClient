using Microsoft.Extensions.Logging;
using NClient.Providers.Serialization;

namespace NClient.Providers.CodeGeneration
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="INClientGenerator"/> instances.
    /// </summary>
    public interface INClientGeneratorProvider
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="ISerializer"/> instance.
        /// </summary>
        INClientGenerator Create(ILogger? logger);
    }
}
