using NClient.Exceptions;

namespace NClient.Providers.Api.Rest.Exceptions.Factories
{
    internal interface IClientArgumentExceptionFactory
    {
        ClientArgumentException ParameterInRouteTemplateIsNull(string parameterName);
    }

    internal class ClientArgumentExceptionFactory : IClientArgumentExceptionFactory
    {
        public ClientArgumentException ParameterInRouteTemplateIsNull(string parameterName) =>
            new($"The parameter '{parameterName}' used in the path cannot be null.");
    }
}
