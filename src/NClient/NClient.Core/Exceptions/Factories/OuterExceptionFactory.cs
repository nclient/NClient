using System;
using System.Net;
using System.Reflection;

namespace NClient.Core.Exceptions.Factories
{
    internal static class OuterExceptionFactory
    {
        public static NClientException RoutePropertyConvertError(string propertyName, string propertyTypeName, string? actualValue) =>
            new($"Property '{propertyName}' has '{propertyTypeName}' type, but value in route '{actualValue}'.");

        public static NClientException PropertyNotFound(string propertyName, string objectName) =>
            new($"Property '{propertyName}' not found in '{objectName}' object type.");

        public static NClientException LimitNestingOfObjects(int limit, string processingObjectName) =>
            new($"The maximum nesting of objects is limited to {limit}. Processing stopped on '{processingObjectName}' object.");

        public static NClientException PropertyValueOfObjectInRouteIsNull(string parameterName, string objectName) =>
            new($"Value of '{parameterName}' property in {objectName} object is null. The value from the path cannot be set.");

        public static NClientException ParameterInRouteTemplateIsNull(string parameterName) =>
            new($"Parameter used in the path cannot be null. Parameter name: {parameterName}");

        public static InvalidAttributeNClientException UsedInvalidAttributeInControllerInterface(string memberName) =>
            new InvalidAttributeNClientException($"An invalid attribute is used for '{memberName}' in controller interface.");
        public static InvalidRouteNClientException RouteParamWithoutTokenInRoute(string clientName, string methodName, string[] paramNames) =>
            new($"Parameters with route attribute '{string.Join(",", paramNames)}' do not have tokens in route template. {GetClientInfo(clientName, methodName)}");

        public static InvalidRouteNClientException TokenFromTemplateNotExists(string clientName, string methodName, string tokenName) =>
            new($"Token '{tokenName}' from route template does not exist. {GetClientInfo(clientName, methodName)}");

        public static InvalidRouteNClientException TemplateParsingError(ArgumentException e) =>
            new(e.Message, e);

        public static InvalidRouteNClientException TemplatePartContainsComplexType(string clientName, string methodName, string parameterName) =>
            new($"Parameter '{parameterName}' cannot be be used in a route template: parameters in a route template must be a primitive type. {GetClientInfo(clientName, methodName)}");

        public static InvalidRouteNClientException TokenNotMatchAnyMethodParameter(string clientName, string methodName, string tokenName) =>
            new($"Token '{tokenName}' in route template does not match any method parameters. {GetClientInfo(clientName, methodName)}");

        public static InvalidRouteNClientException TemplatePartWithoutTokenOrText(string clientName, string methodName) =>
            new($"Template part does not contain a token or text. {GetClientInfo(clientName, methodName)}");

        public static NotSupportedNClientException MultipleAttributeForClientNotSupported(string clientName, string attributeName) =>
            new($"Multiple attributes '{attributeName}' for client are not supported. Client name: {clientName}.");

        public static NotSupportedNClientException MultipleBodyParametersNotSupported(MethodInfo method) =>
            new($"Client method can contain only one body parameter. {GetClientInfo(method)}");

        public static NotSupportedNClientException ComplexTypeInHeaderNotSupported(MethodInfo method, string parameterName) =>
            new($"Headers cannot contain complex types. {GetClientInfo(method, parameterName)}");

        public static NotSupportedNClientException MultipleParameterAttributeNotSupported(MethodInfo method, string parameterName) =>
            new($"Multiple attributes for a method parameter are not supported. {GetClientInfo(method, parameterName)}");

        public static NotSupportedNClientException UsedNotSupportedAttributeForParameter(MethodInfo method, string attributeName, string parameterName) =>
            new($"Attribute '{attributeName}' not supported for parameters. {GetClientInfo(method, parameterName)}");

        public static AttributeNotFoundNClientException MethodAttributeNotFound(Type attributeType, MethodInfo method) =>
            new(attributeType, method);

        public static NotSupportedNClientException MethodAttributeNotSupported(MethodInfo method, string attributeName) =>
            new($"Method attribute '{attributeName}' not supported. {GetClientInfo(method)}");

        public static NotSupportedNClientException MultipleMethodAttributeNotSupported(MethodInfo method) =>
            new($"Multiple attributes for a method are not supported. {GetClientInfo(method)}");

        //TODO: Should contains client and method info
        public static NotSupportedNClientException DictionaryWithComplexTypeOfKeyNotSupported() =>
            new("Dictionary with complex key types cannot be passed through uri query.");

        public static NotSupportedNClientException DictionaryWithComplexTypeOfValueNotSupported() =>
            new("Dictionary with complex value types cannot be passed through uri query.");

        public static NotSupportedNClientException ArrayWithComplexTypeNotSupported() =>
            new("Array with complex types cannot be passed through uri query.");

        public static HttpRequestNClientException HttpRequestFailed(HttpStatusCode statusCode, string? errorMessage) =>
            new(statusCode, errorMessage);

        private static string GetClientInfo(MethodInfo method, string? parameterName = null)
        {
            return GetClientInfo(method.DeclaringType.Name, method.Name, parameterName);
        }

        private static string GetClientInfo(string clientName, string methodName, string? parameterName = null)
        {
            var result = $"Client name: {clientName}. Method name: {methodName}.";
            return parameterName is null ? result : $"{result} Parameter name: {parameterName}.";
        }
    }
}
