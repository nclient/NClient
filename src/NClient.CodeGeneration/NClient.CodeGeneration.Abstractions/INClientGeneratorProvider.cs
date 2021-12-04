using Microsoft.Extensions.Logging;

namespace NClient.CodeGeneration.Abstractions
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="INClientGenerator"/> instances.
    /// </summary>
    public interface INClientGeneratorProvider
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="INClientGenerator"/> instance.
        /// </summary>
        INClientGenerator Create(ILogger? logger);
    }
}
