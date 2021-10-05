using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.Builders
{
    public interface INClientSerializerBuilder<TClient, TRequest, TResponse> where TClient : class
    {
        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> UsingCustomSerializer(ISerializerProvider serializerProvider);
    }
}
