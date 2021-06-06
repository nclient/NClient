using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NClient.AspNetCore.Exceptions.Factories;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors;

namespace NClient.AspNetCore.Binding
{
    internal interface IModelExtender
    {
        void ExtendWithRouteParams(ModelBindingContext bindingContext, object model, IMemberNameSelector memberNameSelector);
    }

    internal class ModelExtender : IModelExtender
    {
        private readonly IObjectMemberManager _objectMemberManager;
        private readonly IControllerValidationExceptionFactory _controllerValidationExceptionFactory;

        public ModelExtender(
            IObjectMemberManager objectMemberManager,
            IControllerValidationExceptionFactory controllerValidationExceptionFactory)
        {
            _objectMemberManager = objectMemberManager;
            _controllerValidationExceptionFactory = controllerValidationExceptionFactory;
        }

        public void ExtendWithRouteParams(ModelBindingContext bindingContext, object model, IMemberNameSelector memberNameSelector)
        {
            foreach (var routeParameter in bindingContext.ActionContext.RouteData.Values
                .Where(routeDataValue => _objectMemberManager.IsMemberPath(routeDataValue.Key)))
            {
                var (objectName, memberPath) = _objectMemberManager.ParseNextPath(routeParameter.Key);
                if (!objectName.Equals(bindingContext.ModelName) && !objectName.Equals(bindingContext.OriginalModelName))
                    throw _controllerValidationExceptionFactory.RouteParameterNotMatchModel(routeParameter.Key, bindingContext.ModelName);
                _objectMemberManager.SetValue(model, (string)routeParameter.Value, memberPath!, memberNameSelector);
            }
        }
    }
}
