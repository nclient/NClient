using System;
using System.Net;
using System.Reflection;

namespace NClient.Core.Exceptions.Factories
{
    internal static class OuterExceptionFactory
    {
        public static InvalidRouteNClientException TokenFromTemplateNotExists(MethodInfo method, string tokenName) =>
            new($"Token '{tokenName}' from route template does not exist. {GetClientInfo(method)}");

        public static InvalidRouteNClientException TemplateParsingError(ArgumentException e) =>
            new(e.Message, e);

        public static InvalidRouteNClientException TemplatePartContainsComplexType(MethodInfo method, string parameterName) =>
            new($"Parameter '{parameterName}' cannot be be used in a route template: parameters in a route template must be a primitive type. {GetClientInfo(method)}");

        public static InvalidRouteNClientException TokenNotMatchAnyMethodParameter(MethodInfo method, string tokenName) =>
            new($"Token '{tokenName}' in route template does not match any method parameters. {GetClientInfo(method)}");

        public static InvalidRouteNClientException TemplatePartWithoutTokenOrText(MethodInfo method) =>
            new($"Template part does not contain a token or text. {GetClientInfo(method)}");

        public static AttributeNotFoundNClientException ClientAttributeNotFound(Type attributeType, Type client) =>
            new(attributeType, client);

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
        
        public static HttpRequestNClientException HttpRequestFailed(HttpStatusCode statusCode, string errorMessage) =>
            new(statusCode, errorMessage);

        private static string GetClientInfo(MethodInfo method, string? parameterName = null)
        {
            var result = $"Client name: {method.DeclaringType.Name}. Method name: {method.Name}.";
            return parameterName is null ? result : $"{result} Parameter name: {parameterName}.";
        }
    }
}
