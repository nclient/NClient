using NClient.Exceptions;

namespace NClient.Core.Helpers.ObjectToKeyValueConverters.Factories
{
    internal interface IObjectToKeyValueConverterExceptionFactory
    {
        ClientValidationException DictionaryWithComplexTypeOfKeyNotSupported();
        ClientValidationException DictionaryWithComplexTypeOfValueNotSupported();
        ClientValidationException ArrayWithComplexTypeNotSupported();
    }
}
