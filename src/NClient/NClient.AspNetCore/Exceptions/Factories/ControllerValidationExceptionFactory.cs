namespace NClient.AspNetCore.Exceptions.Factories
{
    internal interface IControllerValidationExceptionFactory
    {
        ControllerValidationException ControllerAttributeCannotBeCreated(string attributeName);
        ControllerValidationException ModelNotFoundForRouteTemplateToken(string tokenName);
        ControllerValidationException ControllerImplementsMultipleNClientInterfaces(string controllerName);
        ControllerValidationException ControllerInterfaceNotFound(string controllerName);
        ControllerValidationException ControllersNotFound();
    }

    internal class ControllerValidationExceptionFactory : IControllerValidationExceptionFactory
    {
        public ControllerValidationException ControllerAttributeCannotBeCreated(string attributeName) => 
            new($"The controller attribute '{attributeName}' has a constructor that cannot be used to create an attribute.");
        
        public ControllerValidationException ModelNotFoundForRouteTemplateToken(string tokenName) =>
            new($"The model was not found for the '{tokenName}' route template token.");

        public ControllerValidationException ControllerImplementsMultipleNClientInterfaces(string controllerName) =>
            new($"The controller '{controllerName}' must implement a single NClient interface.");

        public ControllerValidationException ControllerInterfaceNotFound(string controllerName) =>
            new($"NClient interface for controller '{controllerName}' not found.");

        public ControllerValidationException ControllersNotFound() =>
            new("NClient controllers not found.");
    }
}
