using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using NClient.AspNetCore.Controllers.Models;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.Mappers;

namespace NClient.AspNetCore.Controllers
{
    internal interface IVirtualControllerGenerator
    {
        IEnumerable<VirtualControllerInfo> Create(IEnumerable<NClientControllerInfo> nclientControllers);
    }

    internal class VirtualControllerGenerator : IVirtualControllerGenerator
    {
        private readonly IAttributeMapper _attributeMapper;
        private readonly IGuidProvider _guidProvider;

        public VirtualControllerGenerator(IAttributeMapper attributeMapper, IGuidProvider guidProvider)
        {
            _attributeMapper = attributeMapper;
            _guidProvider = guidProvider;
        }

        public IEnumerable<VirtualControllerInfo> Create(IEnumerable<NClientControllerInfo> nclientControllers)
        {
            var dynamicAssemblyName = new AssemblyName($"{NClientAssemblyNames.NClientDynamicControllerProxies}.{_guidProvider.Create()}");
            var dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(dynamicAssemblyName, AssemblyBuilderAccess.Run);
            var dynamicModule = dynamicAssembly.DefineDynamicModule(dynamicAssemblyName.Name!);

            foreach (var nclientController in nclientControllers)
            {
                yield return Create(dynamicModule, nclientController);
            }
        }

        private VirtualControllerInfo Create(ModuleBuilder moduleBuilder, NClientControllerInfo nclientController)
        {
            var typeBuilder = moduleBuilder.DefineType(
                name: $"{NClientAssemblyNames.NClientDynamicControllerProxies}.{nclientController.ControllerType.Name}",
                attr: TypeAttributes.Public,
                parent: typeof(ControllerBase));

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            var interfaceAttributes = GetAttributes(nclientController.InterfaceType);
            foreach (var interfaceAttribute in interfaceAttributes)
            {
                typeBuilder.SetCustomAttribute(CreateAttribute(interfaceAttribute));
            }

            var interfaceMethods = nclientController.InterfaceType.GetMethods();
            foreach (var interfaceMethod in interfaceMethods)
            {
                var methodBuilder = DefineMethod(typeBuilder, interfaceMethod);

                var interfaceMethodAttributes = GetAttributes(interfaceMethod);
                foreach (var interfaceMethodAttribute in interfaceMethodAttributes)
                {
                    methodBuilder.SetCustomAttribute(CreateAttribute(interfaceMethodAttribute));
                }
            }

            return new VirtualControllerInfo(typeBuilder.CreateTypeInfo(), nclientController.ControllerType);
        }

        private Attribute[] GetAttributes(ICustomAttributeProvider attributeProvider)
        {
            var attributes = attributeProvider.GetCustomAttributes(inherit: false).Cast<Attribute>();
            return attributes
                .Select(x =>
                {
                    if (x.GetType().Assembly.FullName.StartsWith("Microsoft.AspNetCore"))
                        throw OuterExceptionFactory.UsedInvalidAttributeInControllerInterface(attributeProvider.GetType().FullName);
                    return _attributeMapper.TryMap(x);
                })
                .Where(x => x is not null)
                .ToArray()!;
        }

        private static CustomAttributeBuilder CreateAttribute(Attribute attribute)
        {
            return attribute switch
            {
                IRouteTemplateProvider x => CreateRouteTemplateProviderAttribute(x),
                ApiVersionsBaseAttribute x => CreateApiVersionsBaseAttribute(x),
                _ => CreateCustomAttribute(attribute)
            };
        }

        private static CustomAttributeBuilder CreateRouteTemplateProviderAttribute(IRouteTemplateProvider attribute)
        {
            var attributeProps = GetProperties(attribute);
            var attributePropValues = attributeProps.Select(x => x.GetValue(attribute)).ToArray();
            var attributeFields = GetFields(attribute);
            var attributeFieldValues = attributeFields.Select(x => x.GetValue(attribute)).ToArray();

            if (attribute.Template is null)
                return new CustomAttributeBuilder(
                    attribute.GetType().GetConstructors().First(),
                    Array.Empty<object>(),
                    attributeProps,
                    attributePropValues!,
                    attributeFields,
                    attributeFieldValues!);

            return CreateCustomAttribute((Attribute)attribute);
        }

        private static CustomAttributeBuilder CreateApiVersionsBaseAttribute(ApiVersionsBaseAttribute attribute)
        {
            var attributeProps = GetProperties(attribute);
            var attributePropValues = attributeProps.Select(x => x.GetValue(attribute)).ToArray();
            var attributeFields = GetFields(attribute);
            var attributeFieldValues = attributeFields.Select(x => x.GetValue(attribute)).ToArray();

            return new CustomAttributeBuilder(
                attribute.GetType().GetConstructors().Last(),
                new[] { attribute.Versions.Single().ToString() },
                attributeProps,
                attributePropValues!,
                attributeFields,
                attributeFieldValues!);
        }

        private static CustomAttributeBuilder CreateCustomAttribute(Attribute attribute)
        {
            var attributeCtor = attribute.GetType().GetConstructors().Last();
            var attributeCtorParamNames = new HashSet<string>(attributeCtor.GetParameters().Select(x => x.Name));
            var attributeCtorParamValue = attribute.GetType()
                .GetProperties(bindingAttr: BindingFlags.Public | BindingFlags.Instance)
                .Where(x => attributeCtorParamNames.Contains(x.Name, StringComparer.OrdinalIgnoreCase))
                .Select(x => x.GetValue(attribute))
                .ToArray();

            var attributeProps = GetProperties(attribute);
            var attributePropValues = attributeProps.Select(x => x.GetValue(attribute)).ToArray();
            var attributeFields = GetFields(attribute);
            var attributeFieldValues = attributeFields.Select(x => x.GetValue(attribute)).ToArray();

            return new CustomAttributeBuilder(
                attributeCtor,
                attributeCtorParamValue,
                attributeProps,
                attributePropValues!,
                attributeFields,
                attributeFieldValues!);
        }

        private MethodBuilder DefineMethod(TypeBuilder typeBuilder, MethodInfo methodInfo)
        {
            var methodParams = methodInfo.GetParameters();
            var methodBuilder = typeBuilder.DefineMethod(
                methodInfo.Name,
                attributes: MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                methodInfo.ReturnType,
                methodParams.Select(x => x.ParameterType).ToArray());

            foreach (var (param, index) in methodParams.Select((x, i) => (Param: x, Index: i)))
            {
                var paramBuilder = methodBuilder.DefineParameter(index + 1, param.Attributes, param.Name);
                var paramAttributes = GetAttributes(param);
                foreach (var paramAttribute in paramAttributes)
                {
                    paramBuilder.SetCustomAttribute(CreateCustomAttribute(paramAttribute));
                }
            }

            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ret);

            return methodBuilder;
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
