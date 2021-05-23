using System;
using NClient.Abstractions.Exceptions;

namespace NClient.Core.Exceptions.Factories
{
    internal static class OuterExceptionFactory
    {
        public static RequestNClientException HeaderParamDuplicatesStaticHeader(params string[] headerNames) =>
            new($"Header parameter duplicates static header. Header names: {string.Join(",", headerNames)}");

        public static RequestNClientException ClientNameConsistsOnlyOfSuffixesAndPrefixes() =>
            new($"Client name consists only of suffixes and prefixes.");

        public static NClientException MemberNameConflict(string memberName, string objectName) =>
            new($"Object member '{memberName}' not found in '{objectName}' object type.");

        public static NClientException RoutePropertyConvertError(string memberName, string propertyTypeName, string? actualValue) =>
            new($"Object member '{memberName}' has '{propertyTypeName}' type, but value in route '{actualValue}'.");

        public static NClientException MemberNotFound(string memberName, string objectName) =>
            new($"Object member '{memberName}' not found in '{objectName}' object type.");

        public static NClientException LimitNestingOfObjects(int limit, string processingObjectName) =>
            new($"The maximum nesting of objects is limited to {limit}. Processing stopped on '{processingObjectName}' object.");

        public static NClientException MemberValueOfObjectInRouteIsNull(string memberName, string objectName) =>
            new($"Value of '{memberName}' member in {objectName} object is null. The value from the path cannot be set.");

        public static RequestNClientException ParameterInRouteTemplateIsNull(string parameterName) =>
            new($"Parameter used in the path cannot be null. Parameter name: {parameterName}");

        public static InvalidAttributeNClientException UsedInvalidAttributeInControllerInterface(string memberName) =>
            new($"An invalid attribute is used for '{memberName}' in controller interface.");
        public static InvalidRouteNClientException RouteParamWithoutTokenInRoute(string[] paramNames) =>
            new($"Parameters with route attribute '{string.Join(",", paramNames)}' do not have tokens in route template.");

        public static InvalidRouteNClientException TokenFromTemplateNotExists(string tokenName) =>
            new($"Token '{tokenName}' from route template does not exist.");

        public static InvalidRouteNClientException TemplateParsingError(ArgumentException e) =>
            new(e.Message);

        public static InvalidRouteNClientException TemplatePartContainsComplexType(string parameterName) =>
            new($"Parameter '{parameterName}' cannot be be used in a route template: parameters in a route template must be a primitive type.");

        public static InvalidRouteNClientException TokenNotMatchAnyMethodParameter(string tokenName) =>
            new($"Token '{tokenName}' in route template does not match any method parameters.");

        public static InvalidRouteNClientException TemplatePartWithoutTokenOrText() =>
            new($"Template part does not contain a token or text.");

        public static NotSupportedNClientException MultipleAttributeForClientNotSupported(string attributeName) =>
            new($"Multiple attributes '{attributeName}' for client are not supported.");

        public static NotSupportedNClientException MultipleBodyParametersNotSupported() =>
            new($"Client method can contain only one body parameter.");

        public static NotSupportedNClientException ComplexTypeInHeaderNotSupported(string parameterName) =>
            new($"Headers cannot contain complex types. Parameter name: {parameterName}.");

        public static NotSupportedNClientException MultipleParameterAttributeNotSupported(string parameterName) =>
            new($"Multiple attributes for a method parameter are not supported. Parameter name: {parameterName}.");

        public static NotSupportedNClientException UsedNotSupportedAttributeForParameter(string attributeName, string parameterName) =>
            new($"Attribute '{attributeName}' not supported for parameters ('{parameterName}').");

        public static AttributeNotFoundNClientException MethodAttributeNotFound(string attributeName) =>
            new($"Attribute '{attributeName}' not found.");

        public static NotSupportedNClientException MethodAttributeNotSupported(string attributeName) =>
            new($"Method attribute '{attributeName}' not supported.");

        public static NotSupportedNClientException MultipleMethodAttributeNotSupported() =>
            new($"Multiple attributes for a method are not supported.");

        //TODO: Should contains client and method info
        public static NotSupportedNClientException DictionaryWithComplexTypeOfKeyNotSupported() =>
            new("Dictionary with complex key types cannot be passed through uri query.");

        public static NotSupportedNClientException DictionaryWithComplexTypeOfValueNotSupported() =>
            new("Dictionary with complex value types cannot be passed through uri query.");

        public static NotSupportedNClientException ArrayWithComplexTypeNotSupported() =>
            new("Array with complex types cannot be passed through uri query.");
    }
}
