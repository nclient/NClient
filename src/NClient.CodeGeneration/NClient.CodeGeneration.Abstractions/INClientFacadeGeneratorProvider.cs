using Microsoft.Extensions.Logging;

namespace NClient.CodeGeneration.Abstractions
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="INClientFacadeGenerator"/> instances.
    /// </summary>
    public interface INClientFacadeGeneratorProvider
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="INClientFacadeGenerator"/> instance.
        /// </summary>
        INClientFacadeGenerator Create(ILogger? logger);
    }
}
