using System;

namespace NClient.Abstractions.Serialization
{
    /// <summary>
    /// Provides functionality to serialize objects or value types to JSON and to deserialize JSON into objects or value types.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Parse the text representing a single JSON value into a <paramref name="returnType"/>.
        /// </summary>
        /// <param name="json">The JSON text to parse.</param>
        /// <param name="returnType">The type of the object to convert to and return.</param>
        object? Deserialize(string json, Type returnType);
        /// <summary>
        /// Convert the provided value into a <see cref="string"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type of the <paramref name="value"/> to convert.</typeparam>
        string Serialize<T>(T? value);
    }
}