using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace NClient.AspNetCore.Controllers
{
    public interface IVirtualControllerAttributeBuilder
    {
        CustomAttributeBuilder Build(Attribute attribute);
    }

    public class VirtualControllerAttributeBuilder : IVirtualControllerAttributeBuilder
    {
        public CustomAttributeBuilder Build(Attribute attribute)
        {
            return attribute switch
            {
                IRouteTemplateProvider x => BuildRouteTemplateProviderAttribute(x),
                ApiVersionsBaseAttribute x => BuildApiVersionsBaseAttribute(x),
                _ => BuildCustomAttribute(attribute)
            };
        }

        private static CustomAttributeBuilder BuildRouteTemplateProviderAttribute(IRouteTemplateProvider attribute)
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

        private static CustomAttributeBuilder BuildCustomAttribute(Attribute attribute)
        {
            var attributeCtor = attribute.GetType().GetConstructors().Last();
            var attributeCtorParamNames = new HashSet<string>(attributeCtor.GetParameters().Select(x => x.Name));
            var attributeCtorParamValues = attribute.GetType()
                .GetProperties(bindingAttr: BindingFlags.Public | BindingFlags.Instance)
                .Where(x => attributeCtorParamNames.Contains(x.Name, StringComparer.OrdinalIgnoreCase))
                .Select(x => x.GetValue(attribute))
                .ToArray();

            return BuildAttribute(attributeCtor, attributeCtorParamValues, attribute);
        }

        private static CustomAttributeBuilder BuildAttribute(
            ConstructorInfo attributeCtor, object[] attributeCtorParamValues, Attribute attribute)
        {
            var attributeProps = GetProperties(attribute);
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

        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType()
                .GetProperties(bindingAttr: BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanWrite)
                .ToArray();
        }

        private static FieldInfo[] GetFields(object obj)
        {
            return obj.GetType().GetFields(bindingAttr: BindingFlags.Public | BindingFlags.Instance);
        }
    }
}