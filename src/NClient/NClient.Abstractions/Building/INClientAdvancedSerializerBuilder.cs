using NClient.Providers.Serialization;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientAdvancedSerializerBuilder<TClient, TRequest, TResponse>
        where TClient : class
    {
        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> UsingCustomSerializer(ISerializerProvider serializerProvider);
    }
}
