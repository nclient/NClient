using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IVirtualControllerAttributeBuilder _virtualControllerAttributeBuilder;
        private readonly IAttributeMapper _attributeMapper;
        private readonly IGuidProvider _guidProvider;

        public VirtualControllerGenerator(
            IVirtualControllerAttributeBuilder virtualControllerAttributeBuilder,
            IAttributeMapper attributeMapper,
            IGuidProvider guidProvider)
        {
            _virtualControllerAttributeBuilder = virtualControllerAttributeBuilder;
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
                typeBuilder.SetCustomAttribute(_virtualControllerAttributeBuilder.Build(interfaceAttribute));
            }

            var interfaceMethods = nclientController.InterfaceType.GetMethods();
            foreach (var interfaceMethod in interfaceMethods)
            {
                var methodBuilder = DefineMethod(typeBuilder, interfaceMethod);

                var interfaceMethodAttributes = GetAttributes(interfaceMethod);
                foreach (var interfaceMethodAttribute in interfaceMethodAttributes)
                {
                    methodBuilder.SetCustomAttribute(_virtualControllerAttributeBuilder.Build(interfaceMethodAttribute));
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
                    paramBuilder.SetCustomAttribute(_virtualControllerAttributeBuilder.Build(paramAttribute));
                }
            }

            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ret);

            return methodBuilder;
        }
    }
}
