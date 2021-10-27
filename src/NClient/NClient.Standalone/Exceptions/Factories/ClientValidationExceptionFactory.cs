using NClient.Exceptions;

namespace NClient.Standalone.Exceptions.Factories
{
    internal interface IClientValidationExceptionFactory
    {
        ClientValidationException HeaderParamDuplicatesStaticHeader(params string[] headerNames);
        ClientValidationException MultipleAttributeForClientNotSupported(string attributeName);
        ClientValidationException MultipleParameterAttributeNotSupported(string parameterName);
        ClientValidationException MethodAttributeNotFound(string attributeName);
        ClientValidationException MethodAttributeNotSupported(string attributeName);
        ClientValidationException MultipleMethodAttributeNotSupported();
    }

    internal class ClientValidationExceptionFactory : IClientValidationExceptionFactory
    {
        public ClientValidationException HeaderParamDuplicatesStaticHeader(params string[] headerNames) =>
            new($"Header parameters duplicate static metadatas. Header names: {string.Join(",", headerNames)}");

        public ClientValidationException ClientNameConsistsOnlyOfSuffixesAndPrefixes() =>
            new("The client name consists only of suffixes and/or prefixes.");

        public ClientValidationException MultipleAttributeForClientNotSupported(string attributeName) =>
            new($"Multiple '{attributeName}' attributes for client are not supported.");

        public ClientValidationException MultipleParameterAttributeNotSupported(string parameterName) =>
            new($"Multiple attributes for a method parameter are not supported. Parameter name: {parameterName}.");

        public ClientValidationException MethodAttributeNotFound(string attributeName) =>
            new($"The attribute '{attributeName}' not found.");

        public ClientValidationException MethodAttributeNotSupported(string attributeName) =>
            new($"The method attribute '{attributeName}' not supported.");

        public ClientValidationException MultipleMethodAttributeNotSupported() =>
            new("Multiple attributes for a method not supported.");
    }
}
