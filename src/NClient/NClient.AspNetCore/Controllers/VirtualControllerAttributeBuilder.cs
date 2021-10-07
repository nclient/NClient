using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using NClient.AspNetCore.Exceptions.Factories;
using NClient.Core.Helpers.ObjectMemberManagers;

namespace NClient.AspNetCore.Controllers
{
    internal interface IVirtualControllerAttributeBuilder
    {
        CustomAttributeBuilder? TryBuild(Attribute attribute);
    }

    internal class VirtualControllerAttributeBuilder : IVirtualControllerAttributeBuilder
    {
        private readonly IObjectMemberManager _objectMemberManager;
        private readonly IControllerValidationExceptionFactory _controllerValidationExceptionFactory;

        public VirtualControllerAttributeBuilder(
            IObjectMemberManager objectMemberManager,
            IControllerValidationExceptionFactory controllerValidationExceptionFactory)
        {
            _objectMemberManager = objectMemberManager;
            _controllerValidationExceptionFactory = controllerValidationExceptionFactory;
        }
        
        public CustomAttributeBuilder? TryBuild(Attribute attribute)
        {
            return attribute switch
            {
                IRouteTemplateProvider x => BuildRouteTemplateProviderAttribute(x),
                ApiVersionsBaseAttribute x => BuildApiVersionsBaseAttribute(x),
                _ => TryBuildCustomAttribute(attribute)
            };
        }

        private CustomAttributeBuilder BuildRouteTemplateProviderAttribute(IRouteTemplateProvider attribute)
        {
            if (attribute.Template is not null)
                return BuildCustomAttribute((Attribute)attribute);

            var attributeCtor = attribute.GetType().GetConstructors().First();
            var attributeCtorParamValues = Array.Empty<object>();
            return BuildAttribute(attributeCtor, attributeCtorParamValues, (Attribute)attribute);
        }

        private static CustomAttributeBuilder BuildApiVersionsBaseAttribute(ApiVersionsBaseAttribute attribute)
        {
            var attributeCtor = attribute.GetType().GetConstructors().Last();
            var attributeCtorParamValues = new object[] { attribute.Versions.Single().ToString() };
            return BuildAttribute(attributeCtor, attributeCtorParamValues, attribute);
        }

        private CustomAttributeBuilder? TryBuildCustomAttribute(Attribute attribute)
        {
            try
            {
                return BuildCustomAttribute(attribute);
            }
            catch
            {
                return null;
            }
        }

        private CustomAttributeBuilder BuildCustomAttribute(Attribute attribute)
        {
            var attributeCtor = attribute.GetType().GetConstructors().Last();
            var attributeCtorParamNames = attributeCtor.GetParameters().Select(x => x.Name!);
            var propsAndFields = GetProperties(attribute)
                .Concat(GetFields(attribute).Cast<MemberInfo>())
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);
            var attributeCtorParamValues = attributeCtorParamNames
                .Select(x =>
                {
                    if (!propsAndFields.TryGetValue(x, out var memberInfo))
                        throw _controllerValidationExceptionFactory.ControllerAttributeCannotBeCreated(attribute.GetType().Name);
                    return _objectMemberManager.GetValue(memberInfo, attribute);
                })
                .ToArray();

            return BuildAttribute(attributeCtor, attributeCtorParamValues!, attribute);
        }

        private static CustomAttributeBuilder BuildAttribute(
            ConstructorInfo attributeCtor, object[] attributeCtorParamValues, Attribute attribute)
        {
            var attributeProps = GetCanWriteProperties(attribute);
            var attributePropValues = attributeProps.Select(x => x.GetValue(attribute)).ToArray();
            var attributeFields = GetFields(attribute);
            var attributeFieldValues = attributeFields.Select(x => x.GetValue(attribute)).ToArray();

            return new CustomAttributeBuilder(
                attributeCtor,
                attributeCtorParamValues,
                attributeProps,
                attributePropValues,
                attributeFields,
                attributeFieldValues);
        }
        
        private static PropertyInfo[] GetCanWriteProperties(object obj)
        {
            return GetProperties(obj)
                .Where(x => x.CanWrite)
                .ToArray();
        }

        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType()
                .GetProperties(bindingAttr: BindingFlags.Public | BindingFlags.Instance)
                .ToArray();
        }

        private static FieldInfo[] GetFields(object obj)
        {
            return obj.GetType().GetFields(bindingAttr: BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
