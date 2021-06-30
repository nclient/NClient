using System;
using NClient.Exceptions;

namespace NClient.Core.Exceptions.Factories
{
    internal interface IClientValidationExceptionFactory
    {
        ClientValidationException HeaderParamDuplicatesStaticHeader(params string[] headerNames);
        ClientValidationException ClientNameConsistsOnlyOfSuffixesAndPrefixes();
        ClientValidationException RouteParamWithoutTokenInRoute(params string[] paramNames);
        ClientValidationException SpecialTokenFromTemplateNotExists(string tokenName);
        ClientValidationException TemplateParsingError(ArgumentException e);
        ClientValidationException TemplatePartContainsComplexType(string parameterName);
        ClientValidationException UsedVersionTokenButVersionAttributeNotFound();
        ClientValidationException TokenNotMatchAnyMethodParameter(string tokenName);
        ClientValidationException TemplatePartWithoutTokenOrText();
        ClientValidationException MultipleAttributeForClientNotSupported(string attributeName);
        ClientValidationException MultipleBodyParametersNotSupported();
        ClientValidationException ComplexTypeInHeaderNotSupported(string parameterName);
        ClientValidationException MultipleParameterAttributeNotSupported(string parameterName);
        ClientValidationException MethodAttributeNotFound(string attributeName);
        ClientValidationException MethodAttributeNotSupported(string attributeName);
        ClientValidationException MultipleMethodAttributeNotSupported();
        ClientValidationException DictionaryWithComplexTypeOfKeyNotSupported();
        ClientValidationException DictionaryWithComplexTypeOfValueNotSupported();
        ClientValidationException ArrayWithComplexTypeNotSupported();
    }

    internal class ClientValidationExceptionFactory : IClientValidationExceptionFactory
    {
        public ClientValidationException HeaderParamDuplicatesStaticHeader(params string[] headerNames) =>
            new($"Header parameters duplicate static headers. Header names: {string.Join(",", headerNames)}");

        public ClientValidationException ClientNameConsistsOnlyOfSuffixesAndPrefixes() =>
            new($"The client name consists only of suffixes and/or prefixes.");

        public ClientValidationException RouteParamWithoutTokenInRoute(params string[] paramNames) =>
            new($"Parameters with route attribute '{string.Join(",", paramNames)}' do not have tokens in route template.");

        public ClientValidationException SpecialTokenFromTemplateNotExists(string tokenName) =>
            new($"The special token '{tokenName}' from route template does not exist.");

        public ClientValidationException TemplateParsingError(ArgumentException e) =>
            new(e.Message);

        public ClientValidationException TemplatePartContainsComplexType(string parameterName) =>
            new($"The parameter '{parameterName}' cannot be be used in a route template: parameters in a route template must be a primitive type.");

        public ClientValidationException UsedVersionTokenButVersionAttributeNotFound() =>
            new($"The token 'version' is used, but VersionAttribute not found.");

        public ClientValidationException TokenNotMatchAnyMethodParameter(string tokenName) =>
            new($"The token '{tokenName}' in route template does not match any method parameters.");

        public ClientValidationException TemplatePartWithoutTokenOrText() =>
            new("The template part does not contain a token or text.");

        public ClientValidationException MultipleAttributeForClientNotSupported(string attributeName) =>
            new($"Multiple '{attributeName}' attributes for client are not supported.");

        public ClientValidationException MultipleBodyParametersNotSupported() =>
            new("Client method must contain no more than one body parameter.");

        public ClientValidationException ComplexTypeInHeaderNotSupported(string parameterName) =>
            new($"Headers cannot contain custom types. Parameter name: {parameterName}.");

        public ClientValidationException MultipleParameterAttributeNotSupported(string parameterName) =>
            new($"Multiple attributes for a method parameter are not supported. Parameter name: {parameterName}.");

        public ClientValidationException MethodAttributeNotFound(string attributeName) =>
            new($"The attribute '{attributeName}' not found.");

        public ClientValidationException MethodAttributeNotSupported(string attributeName) =>
            new($"The method attribute '{attributeName}' not supported.");

        public ClientValidationException MultipleMethodAttributeNotSupported() =>
            new("Multiple attributes for a method not supported.");

        public ClientValidationException DictionaryWithComplexTypeOfKeyNotSupported() =>
            new("Dictionary with custom type keys cannot be passed through uri query.");

        public ClientValidationException DictionaryWithComplexTypeOfValueNotSupported() =>
            new("Dictionary with custom type values cannot be passed through uri query.");

        public ClientValidationException ArrayWithComplexTypeNotSupported() =>
            new("Array with custom types cannot be passed through uri query.");
    }
}
