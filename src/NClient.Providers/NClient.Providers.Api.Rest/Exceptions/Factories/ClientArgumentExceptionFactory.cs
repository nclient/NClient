using NClient.Exceptions;

namespace NClient.Providers.Api.Rest.Exceptions.Factories
{
    public interface IClientArgumentExceptionFactory
    {
        ClientArgumentException ParameterInRouteTemplateIsNull(string parameterName);
    }

    public class ClientArgumentExceptionFactory : IClientArgumentExceptionFactory
    {
        public ClientArgumentException ParameterInRouteTemplateIsNull(string parameterName) =>
            new($"The parameter '{parameterName}' used in the path cannot be null.");
    }
}
