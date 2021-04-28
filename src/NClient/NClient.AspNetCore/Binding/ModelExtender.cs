using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NClient.AspNetCore.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors;

namespace NClient.AspNetCore.Binding
{
    internal static class ModelExtender
    {
        private static readonly IObjectMemberManager ObjectMemberManager = new ObjectMemberManager();

        public static void ExtendWithRouteParams(ModelBindingContext bindingContext, object model, IMemberNameSelector memberNameSelector)
        {
            foreach (var routeParameter in bindingContext.ActionContext.RouteData.Values
                .Where(routeDataValue => ObjectMemberManager.IsMemberPath(routeDataValue.Key)))
            {
                var (objectName, memberPath) = ObjectMemberManager.ParseNextPath(routeParameter.Key);
                if (!objectName.Equals(bindingContext.ModelName) && !objectName.Equals(bindingContext.OriginalModelName))
                    throw OuterAspNetExceptionFactory.RouteParameterNotMatchModel(routeParameter.Key, bindingContext.ModelName);
                ObjectMemberManager.SetValue(model, (string)routeParameter.Value, memberPath!, memberNameSelector);
            }
        }
    }
}
