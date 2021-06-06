namespace NClient.AspNetCore.Exceptions.Factories
{
    internal static class ControllerValidationExceptionFactory
    {
        public static ControllerValidationException RouteParameterNotMatchModel(string parameterName, string modelName) =>
            new($"Parameter '{parameterName}' in route template does not match '{modelName}' model.");

        public static ControllerValidationException ControllerCanHaveOnlyOneInterface(string controllerName) =>
            new($"Controller '{controllerName}' can have only one NClient interface.");

        public static ControllerValidationException ControllerInterfaceNotFound(string controllerName) =>
            new($"NClient interface for controller '{controllerName}' not found.");

        public static ControllerValidationException ControllersNotFound() =>
            new($"NClient controllers not found.");
    }
}
