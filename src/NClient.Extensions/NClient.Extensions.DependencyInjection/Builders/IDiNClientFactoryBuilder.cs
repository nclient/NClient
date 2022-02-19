using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedTypeParameter
// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    /// <summary>
    /// A builder for configuring NClient factory instances.
    /// </summary>
    public interface IDiNClientFactoryBuilder<TRequest, TResult>
    {
        /// <summary>
        /// Gets the name of the client factory configured by this builder.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the application service collection.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
