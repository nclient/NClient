using NClient.Providers.Serialization;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientFactoryAdvancedSerializationBuilder<TRequest, TResponse>
    {
        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> UsingCustomSerializer(ISerializerProvider serializerProvider);
    }
}
