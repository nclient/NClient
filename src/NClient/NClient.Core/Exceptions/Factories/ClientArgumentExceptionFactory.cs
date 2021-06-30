namespace NClient.Core.Exceptions.Factories
{
    public interface IClientArgumentExceptionFactory
    {
        ClientArgumentException ParameterInRouteTemplateIsNull(string parameterName);
    }

    public class ClientArgumentExceptionFactory : IClientArgumentExceptionFactory
    {
        public ClientArgumentException ParameterInRouteTemplateIsNull(string parameterName) =>
            new ClientArgumentException($"The parameter '{parameterName}' used in the path cannot be null.");
    }
}