namespace NClient.AspNetCore.Exceptions.Factories
{
    internal interface IControllerValidationExceptionFactory
    {
        ControllerValidationException UsedAspNetCoreAttributeInControllerInterface(string memberName);
        ControllerValidationException ModelNotFoundForRouteTemplateToken(string tokenName);
        ControllerValidationException ControllerImplementsMultipleNClientInterfaces(string controllerName);
        ControllerValidationException ControllerInterfaceNotFound(string controllerName);
        ControllerValidationException ControllersNotFound();
    }

    internal class ControllerValidationExceptionFactory : IControllerValidationExceptionFactory
    {
        public ControllerValidationException UsedAspNetCoreAttributeInControllerInterface(string memberName) =>
            new($"Native ASP.NET Core attributes cannot be used in NClient controller interfaces. Attribute name: '{memberName}'.");

        public ControllerValidationException ModelNotFoundForRouteTemplateToken(string tokenName) =>
            new($"The model was not found for the '{tokenName}' route template token.");

        public ControllerValidationException ControllerImplementsMultipleNClientInterfaces(string controllerName) =>
            new($"The controller '{controllerName}' must implement a single NClient interface.");

        public ControllerValidationException ControllerInterfaceNotFound(string controllerName) =>
            new($"NClient interface for controller '{controllerName}' not found.");

        public ControllerValidationException ControllersNotFound() =>
            new($"NClient controllers not found.");
    }
}
