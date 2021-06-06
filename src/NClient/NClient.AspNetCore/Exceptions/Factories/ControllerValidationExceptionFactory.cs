namespace NClient.AspNetCore.Exceptions.Factories
{
    internal interface IControllerValidationExceptionFactory
    {
        ControllerValidationException UsedForbiddenAttributeInControllerInterface(string memberName);
        ControllerValidationException RouteParameterNotMatchModel(string parameterName, string modelName);
        ControllerValidationException ControllerCanHaveOnlyOneInterface(string controllerName);
        ControllerValidationException ControllerInterfaceNotFound(string controllerName);
        ControllerValidationException ControllersNotFound();
    }

    internal class ControllerValidationExceptionFactory : IControllerValidationExceptionFactory
    {
        public ControllerValidationException UsedForbiddenAttributeInControllerInterface(string memberName) =>
            new($"An forbidden attribute is used for '{memberName}' in controller interface.");
        
        public ControllerValidationException RouteParameterNotMatchModel(string parameterName, string modelName) =>
            new($"Parameter '{parameterName}' in route template does not match '{modelName}' model.");

        public ControllerValidationException ControllerCanHaveOnlyOneInterface(string controllerName) =>
            new($"Controller '{controllerName}' can have only one NClient interface.");

        public ControllerValidationException ControllerInterfaceNotFound(string controllerName) =>
            new($"NClient interface for controller '{controllerName}' not found.");

        public ControllerValidationException ControllersNotFound() =>
            new($"NClient controllers not found.");
    }
}
