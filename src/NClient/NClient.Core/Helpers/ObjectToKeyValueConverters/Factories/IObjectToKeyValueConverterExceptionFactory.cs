using NClient.Exceptions;

namespace NClient.Core.Helpers.ObjectToKeyValueConverters.Factories
{
    public interface IObjectToKeyValueConverterExceptionFactory
    {
        ClientValidationException DictionaryWithComplexTypeOfKeyNotSupported();
        ClientValidationException DictionaryWithComplexTypeOfValueNotSupported();
        ClientValidationException ArrayWithComplexTypeNotSupported();
    }
}
