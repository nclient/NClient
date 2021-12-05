using Microsoft.Extensions.Logging;

namespace NClient.CodeGeneration.Abstractions
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="INClientInterfaceGenerator"/> instances.
    /// </summary>
    public interface INClientInterfaceGeneratorProvider
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="INClientInterfaceGenerator"/> instance.
        /// </summary>
        INClientInterfaceGenerator Create(ILogger? logger);
    }
}
