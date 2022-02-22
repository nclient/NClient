// ReSharper disable once CheckNamespace

namespace NClient.Providers.Mapping.Results
{
    /// <summary>The container that contains deserialized value or error.</summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <typeparam name="TError">The error type.</typeparam>
    public interface IResult<TValue, TError>
    {
        /// <summary>The deserialized value. The value is null if there is an error.</summary>
        public TValue? Value { get; }
        
        /// <summary>The deserialized error. The value is null if there is no error.</summary>
        public TError? Error { get; }
    }

    /// <summary>The container that contains deserialized value or error.</summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <typeparam name="TError">The error type.</typeparam>
    public class Result<TValue, TError> : IResult<TValue, TError>
    {
        /// <summary>The deserialized value. The value is null if there is an error.</summary>
        public TValue? Value { get; }
        
        /// <summary>The deserialized error. The value is null if there is no error.</summary>
        public TError? Error { get; }
        
        /// <summary>Initializes the container that contains deserialized value or error.</summary>
        /// <param name="value">The deserialized value. The value is null if there is an error.</param>
        /// <param name="error">The deserialized error. The value is null if there is no error.</param>
        public Result(TValue? value, TError? error)
        {
            Value = value;
            Error = error;
        }
        
        /// <summary>Deconstructs result.</summary>
        /// <param name="value">The deserialized value. The value is null if there is an error.</param>
        /// <param name="error">The deserialized error. The value is null if there is no error.</param>
        public void Deconstruct(out TValue? value, out TError? error)
        {
            value = Value;
            error = Error;
        }
    }
}
