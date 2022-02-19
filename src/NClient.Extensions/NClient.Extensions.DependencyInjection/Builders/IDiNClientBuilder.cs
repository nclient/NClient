using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedTypeParameter
// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    /// <summary>
    /// A builder for configuring NClient instances.
    /// </summary>
    public interface IDiNClientBuilder<TClient, TRequest, TResult> where TClient : class
    {
        /// <summary>
        /// Gets the name of the client configured by this builder.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the application service collection.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
