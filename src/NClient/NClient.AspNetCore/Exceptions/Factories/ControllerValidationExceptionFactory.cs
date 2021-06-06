using NClient.Abstractions.Exceptions;
using NClient.Core.Exceptions;
using NClient.Core.Helpers.ObjectMemberManagers.Factories;

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

    internal class ControllerValidationExceptionFactory : IControllerValidationExceptionFactory, IObjectMemberManagerExceptionFactory
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
