using NClient.Abstractions.Exceptions;

namespace NClient.Core.Helpers.ObjectMemberManagers.Factories
{
    public interface IObjectMemberManagerExceptionFactory
    {
        NClientException MemberNameConflict(string memberName, string objectName);
        NClientException MemberNotFound(string memberName, string objectName);
        NClientException MemberValueOfObjectInRouteIsNull(string memberName, string objectName);
        NClientException RoutePropertyConvertError(string memberName, string propertyTypeName, string? actualValue);
        NClientException LimitNestingOfObjects(int limit, string processingObjectName);
    }
}