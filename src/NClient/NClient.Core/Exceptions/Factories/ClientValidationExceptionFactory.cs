using System;
using NClient.Abstractions.Exceptions;
using NClient.Core.Helpers.ObjectMemberManagers.Factories;

namespace NClient.Core.Exceptions.Factories
{
    internal interface IClientValidationExceptionFactory
    {
        ClientValidationException HeaderParamDuplicatesStaticHeader(params string[] headerNames);
        ClientValidationException ClientNameConsistsOnlyOfSuffixesAndPrefixes();
        ClientValidationException ParameterInRouteTemplateIsNull(string parameterName);
        ClientValidationException RouteParamWithoutTokenInRoute(string[] paramNames);
        ClientValidationException TokenFromTemplateNotExists(string tokenName);
        ClientValidationException TemplateParsingError(ArgumentException e);
        ClientValidationException TemplatePartContainsComplexType(string parameterName);
        ClientValidationException UsedVersionTokenButVersionAttributeNotFound();
        ClientValidationException TokenNotMatchAnyMethodParameter(string tokenName);
        ClientValidationException TemplatePartWithoutTokenOrText();
        ClientValidationException MultipleAttributeForClientNotSupported(string attributeName);
        ClientValidationException MultipleBodyParametersNotSupported();
        ClientValidationException ComplexTypeInHeaderNotSupported(string parameterName);
        ClientValidationException MultipleParameterAttributeNotSupported(string parameterName);
        ClientValidationException UsedNotSupportedAttributeForParameter(string attributeName, string parameterName);
        ClientValidationException MethodAttributeNotFound(string attributeName);
        ClientValidationException MethodAttributeNotSupported(string attributeName);
        ClientValidationException MultipleMethodAttributeNotSupported();
        ClientValidationException DictionaryWithComplexTypeOfKeyNotSupported();
        ClientValidationException DictionaryWithComplexTypeOfValueNotSupported();
        ClientValidationException ArrayWithComplexTypeNotSupported();
    }

    internal class ClientValidationExceptionFactory : IClientValidationExceptionFactory, IObjectMemberManagerExceptionFactory
    {
        public ClientValidationException HeaderParamDuplicatesStaticHeader(params string[] headerNames) =>
            new($"Header parameter duplicates static header. Header names: {string.Join(",", headerNames)}");

        public ClientValidationException ClientNameConsistsOnlyOfSuffixesAndPrefixes() =>
            new($"Client name consists only of suffixes and prefixes.");
        
        public ClientValidationException ParameterInRouteTemplateIsNull(string parameterName) =>
            new($"Parameter used in the path cannot be null. Parameter name: {parameterName}");
        
        public ClientValidationException RouteParamWithoutTokenInRoute(string[] paramNames) =>
            new($"Parameters with route attribute '{string.Join(",", paramNames)}' do not have tokens in route template.");

        public ClientValidationException TokenFromTemplateNotExists(string tokenName) =>
            new($"Token '{tokenName}' from route template does not exist.");

        public ClientValidationException TemplateParsingError(ArgumentException e) =>
            new(e.Message);

        public ClientValidationException TemplatePartContainsComplexType(string parameterName) =>
            new($"Parameter '{parameterName}' cannot be be used in a route template: parameters in a route template must be a primitive type.");

        public ClientValidationException UsedVersionTokenButVersionAttributeNotFound() =>
            new($"Token 'version' is used, but VersionAttribute not found.");

        public ClientValidationException TokenNotMatchAnyMethodParameter(string tokenName) =>
            new($"Token '{tokenName}' in route template does not match any method parameters.");

        public ClientValidationException TemplatePartWithoutTokenOrText() =>
            new($"Template part does not contain a token or text.");

        public ClientValidationException MultipleAttributeForClientNotSupported(string attributeName) =>
            new($"Multiple attributes '{attributeName}' for client are not supported.");

        public ClientValidationException MultipleBodyParametersNotSupported() =>
            new($"Client method can contain only one body parameter.");

        public ClientValidationException ComplexTypeInHeaderNotSupported(string parameterName) =>
            new($"Headers cannot contain complex types. Parameter name: {parameterName}.");

        public ClientValidationException MultipleParameterAttributeNotSupported(string parameterName) =>
            new($"Multiple attributes for a method parameter are not supported. Parameter name: {parameterName}.");

        public ClientValidationException UsedNotSupportedAttributeForParameter(string attributeName, string parameterName) =>
            new($"Attribute '{attributeName}' not supported for parameters ('{parameterName}').");

        public ClientValidationException MethodAttributeNotFound(string attributeName) =>
            new($"Attribute '{attributeName}' not found.");

        public ClientValidationException MethodAttributeNotSupported(string attributeName) =>
            new($"Method attribute '{attributeName}' not supported.");

        public ClientValidationException MultipleMethodAttributeNotSupported() =>
            new($"Multiple attributes for a method are not supported.");
        
        public ClientValidationException DictionaryWithComplexTypeOfKeyNotSupported() =>
            new("Dictionary with complex key types cannot be passed through uri query.");

        public ClientValidationException DictionaryWithComplexTypeOfValueNotSupported() =>
            new("Dictionary with complex value types cannot be passed through uri query.");

        public ClientValidationException ArrayWithComplexTypeNotSupported() =>
            new("Array with complex types cannot be passed through uri query.");
        
        
        public NClientException MemberNameConflict(string memberName, string objectName) =>
            new ClientValidationException($"Object member '{memberName}' not found in '{objectName}' object type.");
       
        public NClientException MemberNotFound(string memberName, string objectName) =>
            new ClientValidationException($"Object member '{memberName}' not found in '{objectName}' object type.");

        public NClientException MemberValueOfObjectInRouteIsNull(string memberName, string objectName) =>
            new ClientValidationException($"Value of '{memberName}' member in {objectName} object is null. The value from the path cannot be set.");

        public NClientException RoutePropertyConvertError(string memberName, string propertyTypeName, string? actualValue) =>
            new ClientValidationException($"Object member '{memberName}' has '{propertyTypeName}' type, but value in route '{actualValue}'.");

        public NClientException LimitNestingOfObjects(int limit, string processingObjectName) =>
            new ClientValidationException($"The maximum nesting of objects is limited to {limit}. Processing stopped on '{processingObjectName}' object.");
    }
}
