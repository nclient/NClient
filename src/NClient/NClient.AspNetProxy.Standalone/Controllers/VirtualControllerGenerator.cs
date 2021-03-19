using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using NClient.Core.Attributes;

namespace NClient.AspNetProxy.Controllers
{
    internal interface IVirtualControllerGenerator
    {
        IEnumerable<(Type VirtualControllerType, Type ControllerType)> Create(
            IEnumerable<(Type InterfaceType, Type ControllerType)> interfaceControllerPairs);
    }

    internal class VirtualControllerGenerator : IVirtualControllerGenerator
    {
        private readonly IAttributeMapper _attributeMapper;

        public VirtualControllerGenerator(IAttributeMapper attributeMapper)
        {
            _attributeMapper = attributeMapper;
        }

        public IEnumerable<(Type VirtualControllerType, Type ControllerType)> Create(
            IEnumerable<(Type InterfaceType, Type ControllerType)> interfaceControllerPairs)
        {
            var dynamicAssemblyName = new AssemblyName(NClientAssemblyNames.NClientDynamicControllerProxies);
            var dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(dynamicAssemblyName, AssemblyBuilderAccess.Run);
            var dynamicModule = dynamicAssembly.DefineDynamicModule(dynamicAssemblyName.Name!);

            foreach (var (interfaceType, controllerType) in interfaceControllerPairs)
            {
                yield return Create(dynamicModule, interfaceType, controllerType);
            }
        }

        private (Type VirtualControllerType, Type ControllerType) Create(
            ModuleBuilder moduleBuilder, Type interfaceType, Type controllerType)
        {
            var typeBuilder = moduleBuilder.DefineType(controllerType.Name, attr: TypeAttributes.Public, parent: typeof(ControllerBase));

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            var controllerAttributes = GetAttributes(interfaceType);
            typeBuilder.SetCustomAttribute(CreateCustomAttribute(new ApiControllerAttribute()));
            foreach (var controllerAttribute in controllerAttributes)
            {
                typeBuilder.SetCustomAttribute(CreateCustomAttribute(controllerAttribute));
            }

            var interfaceMethods = interfaceType.GetMethods();
            foreach (var interfaceMethod in interfaceMethods)
            {
                var method = DefineMethod(typeBuilder, interfaceMethod);

                var controllerMethodAttributes = GetAttributes(interfaceMethod); ;
                foreach (var controllerMethodAttribute in controllerMethodAttributes)
                {
                    method.SetCustomAttribute(CreateCustomAttribute(controllerMethodAttribute));
                }
            }

            return (typeBuilder.CreateTypeInfo(), controllerType);
        }

        private Attribute[] GetAttributes(MemberInfo memberInfo)
        {
            var interfaceAttributes = memberInfo.GetCustomAttributes();
            return interfaceAttributes
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x != null)
                .ToArray()!;
        }

        private static CustomAttributeBuilder CreateCustomAttribute(Attribute attribute)
        {
            var attributeCtor = attribute.GetType().GetConstructors().First();
            var attributeCtorParamNames = new HashSet<string>(attributeCtor.GetParameters().Select(x => x.Name));
            var attributeCtorParamValue = attribute.GetType()
                .GetProperties(bindingAttr: BindingFlags.Public | BindingFlags.Instance)
                .Where(x => attributeCtorParamNames.Contains(x.Name, StringComparer.OrdinalIgnoreCase))
                .Select(x => x.GetValue(attribute))
                .ToArray();

            var attributeProps = attribute.GetType()
                .GetProperties(bindingAttr: BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanWrite)
                .ToArray();
            var attributePropValues = attributeProps.Select(x => x.GetValue(attribute)).ToArray();

            var attributeFields = attribute.GetType().GetFields(bindingAttr: BindingFlags.Public | BindingFlags.Instance);
            var attributeFieldValues = attributeFields.Select(x => x.GetValue(attribute)).ToArray();

            return new CustomAttributeBuilder(
                attributeCtor,
                attributeCtorParamValue,
                attributeProps,
                attributePropValues!,
                attributeFields,
                attributeFieldValues!);
        }

        private static MethodBuilder DefineMethod(TypeBuilder typeBuilder, MethodInfo methodInfo)
        {
            var methodParams = methodInfo.GetParameters();
            var methodBuilder = typeBuilder.DefineMethod(
                methodInfo.Name,
                attributes: MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                methodInfo.ReturnType,
                methodParams.Select(x => x.ParameterType).ToArray());

            foreach (var (param, index) in methodParams.Select((x, i) => (Param: x, Index: i)))
            {
                methodBuilder.DefineParameter(index + 1, param.Attributes, param.Name);
            }

            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ret);

            return methodBuilder;
        }
    }
}
