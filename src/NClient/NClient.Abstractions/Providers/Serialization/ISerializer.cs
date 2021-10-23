using System;

namespace NClient.Providers.Serialization
{
    /// <summary>
    /// Provides functionality to serialize objects or value types to serialized string and to deserialize serialized string into objects or value types.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Gets supported content type. 
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Parse the text representing a single serialized value into a <paramref name="returnType"/>.
        /// </summary>
        /// <param name="source">The serialized data to parse.</param>
        /// <param name="returnType">The type of the object to convert to and return.</param>
        object? Deserialize(string source, Type returnType);

        /// <summary>
        /// Convert the provided value into a <see cref="string"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type of the <paramref name="value"/> to convert.</typeparam>
        string Serialize<T>(T? value);
    }
}
