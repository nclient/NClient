// ReSharper disable once CheckNamespace

namespace NClient.Providers.Serialization
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="ISerializer"/> instances.
    /// </summary>
    public interface ISerializerProvider
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="ISerializer"/> instance.
        /// </summary>
        ISerializer Create();
    }
}
