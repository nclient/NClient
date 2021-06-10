using NClient.Abstractions.Exceptions;
using NClient.Core.Exceptions;
using NClient.Core.Helpers.ObjectMemberManagers.Factories;

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

    internal class ControllerValidationExceptionFactory : IControllerValidationExceptionFactory, IObjectMemberManagerExceptionFactory
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


        public NClientException MemberNameConflict(string memberName, string objectName) =>
            new ClientValidationException($"Multiple '{memberName}' members were found in the '{objectName}' object type.");

        public NClientException MemberNotFound(string memberName, string objectName) =>
            new ClientValidationException($"The member '{memberName}' not found in '{objectName}' object type.");

        public NClientException MemberValueOfObjectInRouteIsNull(string memberName, string objectName) =>
            new ClientValidationException($"The value of '{memberName}' member in {objectName} object is null. The value cannot be inserted in the route template.");

        public NClientException RoutePropertyConvertError(string memberName, string propertyTypeName, string? actualValue) =>
            new ClientValidationException($"The object member '{memberName}' has '{propertyTypeName}' type, but value in route is '{actualValue}'.");

        public NClientException LimitNestingOfObjects(int limit, string processingObjectName) =>
            new ClientValidationException($"The maximum nesting of objects is limited to {limit}. Processing stopped on '{processingObjectName}' object.");
    }
}
