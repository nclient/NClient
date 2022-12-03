using System;
using NClient.Exceptions;

namespace NClient.Providers.Api.Rest.Exceptions.Factories
{
    internal interface IClientValidationExceptionFactory
    {
        ClientValidationException ClientNameConsistsOnlyOfSuffixesAndPrefixes();
        ClientValidationException RouteParamWithoutTokenInRoute(params string[] paramNames);
        ClientValidationException SpecialTokenFromTemplateNotExists(string tokenName);
        ClientValidationException TemplateParsingError(ArgumentException e);
        ClientValidationException TemplatePartContainsComplexType(string parameterName);
        ClientValidationException UsedVersionTokenButVersionAttributeNotFound();
        ClientValidationException TokenNotMatchAnyMethodParameter(string tokenName);
        ClientValidationException TemplatePartWithoutTokenOrText();
        ClientValidationException ComplexTypeInHeaderNotSupported(string parameterName);
        ClientValidationException MethodAttributeNotSupported(string attributeName);
    }

    internal class ClientValidationExceptionFactory : IClientValidationExceptionFactory
    { 
        public ClientValidationException ClientNameConsistsOnlyOfSuffixesAndPrefixes() =>
            new("The client name consists only of suffixes and/or prefixes.");

        public ClientValidationException RouteParamWithoutTokenInRoute(params string[] paramNames) =>
            new($"Parameters with route attribute '{string.Join(",", paramNames)}' do not have tokens in route template.");

        public ClientValidationException SpecialTokenFromTemplateNotExists(string tokenName) =>
            new($"The special token '{tokenName}' from route template does not exist.");

        public ClientValidationException TemplateParsingError(ArgumentException e) =>
            new(e.Message);

        public ClientValidationException TemplatePartContainsComplexType(string parameterName) =>
            new($"The parameter '{parameterName}' cannot be be used in a route template: parameters in a route template must be a primitive type.");

        public ClientValidationException UsedVersionTokenButVersionAttributeNotFound() =>
            new("The token 'version' is used, but VersionAttribute not found.");

        public ClientValidationException TokenNotMatchAnyMethodParameter(string tokenName) =>
            new($"The token '{tokenName}' in route template does not match any method parameters.");

        public ClientValidationException TemplatePartWithoutTokenOrText() =>
            new("The template part does not contain a token or text.");

        public ClientValidationException ComplexTypeInHeaderNotSupported(string parameterName) =>
            new($"Headers cannot contain custom types. Parameter name: {parameterName}.");

        public ClientValidationException MethodAttributeNotSupported(string attributeName) =>
            new($"The method attribute '{attributeName}' not supported.");
    }
}
