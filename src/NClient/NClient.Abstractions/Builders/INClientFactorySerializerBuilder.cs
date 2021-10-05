using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.Builders
{
    public interface INClientFactorySerializerBuilder<TRequest, TResponse>
    {
        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> UsingCustomSerializer(ISerializerProvider serializerProvider);
    }
}
