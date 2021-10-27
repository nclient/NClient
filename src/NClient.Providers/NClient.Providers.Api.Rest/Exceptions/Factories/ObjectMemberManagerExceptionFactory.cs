using NClient.Core.Helpers.ObjectMemberManagers.Factories;
using NClient.Exceptions;

namespace NClient.Providers.Api.Rest.Exceptions.Factories
{
    public class ObjectMemberManagerExceptionFactory : IObjectMemberManagerExceptionFactory
    {
        public NClientException MemberNameConflict(string memberName, string objectName) =>
            new ClientValidationException($"Multiple '{memberName}' members were found in the '{objectName}' object type.");

        public NClientException MemberNotFound(string memberName, string objectName) =>
            new ClientValidationException($"The member '{memberName}' not found in '{objectName}' object type.");

        public NClientException LimitNestingOfObjects(int limit, string processingObjectName) =>
            new ClientValidationException($"The maximum nesting of objects is limited to {limit}. Processing stopped on '{processingObjectName}' object.");

        public NClientException MemberValueOfObjectInRouteIsNull(string memberName, string objectName) =>
            new ClientArgumentException($"The value of '{memberName}' member in {objectName} object is null. The value cannot be inserted in the route template.");

        public NClientException RoutePropertyConvertError(string memberName, string propertyTypeName, string? actualValue) =>
            new ClientArgumentException($"The object member '{memberName}' has '{propertyTypeName}' type, but value in route is '{actualValue}'.");
    }
}
