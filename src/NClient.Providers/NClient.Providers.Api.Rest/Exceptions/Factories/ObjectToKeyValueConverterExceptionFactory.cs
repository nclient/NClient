using NClient.Core.Helpers.ObjectToKeyValueConverters.Factories;
using NClient.Exceptions;

namespace NClient.Providers.Api.Rest.Exceptions.Factories
{
    internal class ObjectToKeyValueConverterExceptionFactory : IObjectToKeyValueConverterExceptionFactory
    {
        public ClientValidationException DictionaryWithComplexTypeOfKeyNotSupported() =>
            new("Dictionary with custom type keys cannot be passed through uri query.");

        public ClientValidationException DictionaryWithComplexTypeOfValueNotSupported() =>
            new("Dictionary with custom type values cannot be passed through uri query.");

        public ClientValidationException ArrayWithComplexTypeNotSupported() =>
            new("Array with custom types cannot be passed through uri query.");
    }
}
