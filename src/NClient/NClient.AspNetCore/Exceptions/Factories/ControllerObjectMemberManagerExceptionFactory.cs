using NClient.Core.Helpers.ObjectMemberManagers.Factories;
using NClient.Exceptions;

namespace NClient.AspNetCore.Exceptions.Factories
{
    public class ControllerObjectMemberManagerExceptionFactory : IObjectMemberManagerExceptionFactory
    {
        public NClientException MemberNameConflict(string memberName, string objectName) =>
            new ControllerValidationException($"Multiple '{memberName}' members were found in the '{objectName}' object type.");

        public NClientException MemberNotFound(string memberName, string objectName) =>
            new ControllerValidationException($"The member '{memberName}' not found in '{objectName}' object type.");

        public NClientException LimitNestingOfObjects(int limit, string processingObjectName) =>
            new ControllerValidationException($"The maximum nesting of objects is limited to {limit}. Processing stopped on '{processingObjectName}' object.");

        public NClientException RoutePropertyConvertError(string memberName, string propertyTypeName, string? actualValue) =>
            new ControllerArgumentException($"The object member '{memberName}' has '{propertyTypeName}' type, but value in route is '{actualValue}'.");

        public NClientException MemberValueOfObjectInRouteIsNull(string memberName, string objectName) =>
            new ControllerArgumentException($"The value of '{memberName}' member in {objectName} object is null. The value cannot be inserted in the route template.");
    }
}
