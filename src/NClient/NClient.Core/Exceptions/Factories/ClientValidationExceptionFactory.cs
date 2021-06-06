using System;

namespace NClient.Core.Exceptions.Factories
{
    internal static class ClientValidationExceptionFactory
    {
        public static ClientValidationException HeaderParamDuplicatesStaticHeader(params string[] headerNames) =>
            new($"Header parameter duplicates static header. Header names: {string.Join(",", headerNames)}");

        public static ClientValidationException ClientNameConsistsOnlyOfSuffixesAndPrefixes() =>
            new($"Client name consists only of suffixes and prefixes.");

        public static ClientValidationException MemberNameConflict(string memberName, string objectName) =>
            new($"Object member '{memberName}' not found in '{objectName}' object type.");

        public static ClientValidationException RoutePropertyConvertError(string memberName, string propertyTypeName, string? actualValue) =>
            new($"Object member '{memberName}' has '{propertyTypeName}' type, but value in route '{actualValue}'.");

        public static ClientValidationException MemberNotFound(string memberName, string objectName) =>
            new($"Object member '{memberName}' not found in '{objectName}' object type.");

        public static ClientValidationException LimitNestingOfObjects(int limit, string processingObjectName) =>
            new($"The maximum nesting of objects is limited to {limit}. Processing stopped on '{processingObjectName}' object.");

        public static ClientValidationException MemberValueOfObjectInRouteIsNull(string memberName, string objectName) =>
            new($"Value of '{memberName}' member in {objectName} object is null. The value from the path cannot be set.");

        public static ClientValidationException ParameterInRouteTemplateIsNull(string parameterName) =>
            new($"Parameter used in the path cannot be null. Parameter name: {parameterName}");

        public static ClientValidationException UsedForbiddenAttributeInControllerInterface(string memberName) =>
            new($"An forbidden attribute is used for '{memberName}' in controller interface.");
        public static ClientValidationException RouteParamWithoutTokenInRoute(string[] paramNames) =>
            new($"Parameters with route attribute '{string.Join(",", paramNames)}' do not have tokens in route template.");

        public static ClientValidationException TokenFromTemplateNotExists(string tokenName) =>
            new($"Token '{tokenName}' from route template does not exist.");

        public static ClientValidationException TemplateParsingError(ArgumentException e) =>
            new(e.Message);

        public static ClientValidationException TemplatePartContainsComplexType(string parameterName) =>
            new($"Parameter '{parameterName}' cannot be be used in a route template: parameters in a route template must be a primitive type.");

        public static ClientValidationException UsedVersionTokenButVersionAttributeNotFound() =>
            new($"Token 'version' is used, but VersionAttribute not found.");

        public static ClientValidationException TokenNotMatchAnyMethodParameter(string tokenName) =>
            new($"Token '{tokenName}' in route template does not match any method parameters.");

        public static ClientValidationException TemplatePartWithoutTokenOrText() =>
            new($"Template part does not contain a token or text.");

        public static ClientValidationException MultipleAttributeForClientNotSupported(string attributeName) =>
            new($"Multiple attributes '{attributeName}' for client are not supported.");

        public static ClientValidationException MultipleBodyParametersNotSupported() =>
            new($"Client method can contain only one body parameter.");

        public static ClientValidationException ComplexTypeInHeaderNotSupported(string parameterName) =>
            new($"Headers cannot contain complex types. Parameter name: {parameterName}.");

        public static ClientValidationException MultipleParameterAttributeNotSupported(string parameterName) =>
            new($"Multiple attributes for a method parameter are not supported. Parameter name: {parameterName}.");

        public static ClientValidationException UsedNotSupportedAttributeForParameter(string attributeName, string parameterName) =>
            new($"Attribute '{attributeName}' not supported for parameters ('{parameterName}').");

        public static ClientValidationException MethodAttributeNotFound(string attributeName) =>
            new($"Attribute '{attributeName}' not found.");

        public static ClientValidationException MethodAttributeNotSupported(string attributeName) =>
            new($"Method attribute '{attributeName}' not supported.");

        public static ClientValidationException MultipleMethodAttributeNotSupported() =>
            new($"Multiple attributes for a method are not supported.");
        
        public static ClientValidationException DictionaryWithComplexTypeOfKeyNotSupported() =>
            new("Dictionary with complex key types cannot be passed through uri query.");

        public static ClientValidationException DictionaryWithComplexTypeOfValueNotSupported() =>
            new("Dictionary with complex value types cannot be passed through uri query.");

        public static ClientValidationException ArrayWithComplexTypeNotSupported() =>
            new("Array with complex types cannot be passed through uri query.");
    }
}
