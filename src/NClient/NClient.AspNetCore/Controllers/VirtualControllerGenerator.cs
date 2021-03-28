using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Mappers;

namespace NClient.AspNetCore.Controllers
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
            // TODO: Use GuidProvider instead static Guid.NewGuid()
            var dynamicAssemblyName = new AssemblyName($"{NClientAssemblyNames.NClientDynamicControllerProxies}.{Guid.NewGuid()}");
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
            var typeBuilder = moduleBuilder.DefineType(
                name: $"{NClientAssemblyNames.NClientDynamicControllerProxies}.{controllerType.Name}", 
                attr: TypeAttributes.Public, 
                parent: typeof(ControllerBase));

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            var interfaceAttributes = GetAttributes(interfaceType);
            typeBuilder.SetCustomAttribute(CreateCustomAttribute(new ApiControllerAttribute()));
            foreach (var interfaceAttribute in interfaceAttributes)
            {
                typeBuilder.SetCustomAttribute(CreateCustomAttribute(interfaceAttribute));
            }

            var interfaceMethods = interfaceType.GetMethods();
            foreach (var interfaceMethod in interfaceMethods)
            {
                var methodBuilder = DefineMethod(typeBuilder, interfaceMethod);

                var interfaceMethodAttributes = GetAttributes(interfaceMethod);
                foreach (var interfaceMethodAttribute in interfaceMethodAttributes)
                {
                    methodBuilder.SetCustomAttribute(CreateCustomAttribute(interfaceMethodAttribute));
                }
            }

            return (typeBuilder.CreateTypeInfo(), controllerType);
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

        private static CustomAttributeBuilder CreateCustomAttribute(Attribute attribute)
        {
            var attributeCtor = attribute.GetType().GetConstructors().Last();
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
    }
}
