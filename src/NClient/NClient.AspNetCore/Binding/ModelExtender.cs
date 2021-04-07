using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NClient.AspNetCore.Exceptions.Factories;
using NClient.Core.Helpers;

namespace NClient.AspNetCore.Binding
{
    public static class ModelExtender
    {
        public static void ExtendWithRouteParams(ModelBindingContext bindingContext, object model)
        {
            foreach (var routeParameter in bindingContext.ActionContext.RouteData.Values
                .Where(routeDataValue => ObjectMemberManager.IsMemberPath(routeDataValue.Key)))
            {
                var (objectName, memberPath) = ObjectMemberManager.ParseNextPath(routeParameter.Key);
                if (!objectName.Equals(bindingContext.ModelName))
                    throw OuterAspNetExceptionFactory.RouteParameterNotMatchModel(routeParameter.Key, bindingContext.ModelName);
                ObjectMemberManager.SetMemberValue(model, (string)routeParameter.Value, memberPath!);
            }
        }
    }
}
