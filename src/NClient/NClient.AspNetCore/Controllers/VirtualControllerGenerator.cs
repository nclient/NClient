using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using NClient.Abstractions.Helpers;
using NClient.AspNetCore.Controllers.Models;
using NClient.AspNetCore.Exceptions;
using NClient.AspNetCore.Exceptions.Factories;
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
        private readonly IControllerValidationExceptionFactory _controllerValidationExceptionFactory;
        private readonly IGuidProvider _guidProvider;

        public VirtualControllerGenerator(
            IVirtualControllerAttributeBuilder virtualControllerAttributeBuilder,
            IAttributeMapper attributeMapper,
            IControllerValidationExceptionFactory controllerValidationExceptionFactory,
            IGuidProvider guidProvider)
        {
            _virtualControllerAttributeBuilder = virtualControllerAttributeBuilder;
            _attributeMapper = attributeMapper;
            _controllerValidationExceptionFactory = controllerValidationExceptionFactory;
            _guidProvider = guidProvider;
        }

        public IEnumerable<VirtualControllerInfo> Create(IEnumerable<NClientControllerInfo> nclientControllers)
        {
            var dynamicAssemblyName = new AssemblyName($"{NClientAssemblyNames.NClientDynamicControllerProxies}.{_guidProvider.Create()}");
            var dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(dynamicAssemblyName, AssemblyBuilderAccess.Run);
            var dynamicModule = dynamicAssembly.DefineDynamicModule(dynamicAssemblyName.Name!);

            foreach (var nclientController in nclientControllers)
            {
                VirtualControllerInfo virtualController;
                try
                {
                    virtualController = Create(dynamicModule, nclientController);
                }
                catch (ControllerException e)
                {
                    e.ControllerType = nclientController.ControllerType;
                    e.InterfaceType = nclientController.InterfaceType;
                    throw;
                }

                yield return virtualController;
            }
        }

        private VirtualControllerInfo Create(ModuleBuilder moduleBuilder, NClientControllerInfo nclientController)
        {
            var typeBuilder = moduleBuilder.DefineType(
                name: $"{NClientAssemblyNames.NClientDynamicControllerProxies}.{nclientController.ControllerType.Name}",
                attr: TypeAttributes.Public,
                parent: typeof(ControllerBase));

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            var interfaceAttributes = GetInterfaceAttributes(nclientController.InterfaceType);
            var controllerAttributes = GetControllerAttributes(nclientController.ControllerType);
            foreach (var virtualControllerAttribute in interfaceAttributes.Concat(controllerAttributes))
            {
                if (_virtualControllerAttributeBuilder.TryBuild(virtualControllerAttribute) is { } attributeBuilder)
                    typeBuilder.SetCustomAttribute(attributeBuilder);
            }

            var methodEqualityComparer = new MethodInfoEqualityComparer();
            var interfaceMethods = nclientController.InterfaceType.GetMethods();
            var controllerMethods = nclientController.ControllerType.GetMethods();
            var virtualControllerMethods = interfaceMethods
                .Select(im => (MethodFromInterface: im, MethodFromController: controllerMethods.Single(cm => methodEqualityComparer.Equals(im, cm))))
                .ToArray();
            foreach (var (interfaceMethod, controllerMethod) in virtualControllerMethods)
            {
                var methodBuilder = DefineMethod(typeBuilder, interfaceMethod, controllerMethod);

                var interfaceMethodAttributes = GetInterfaceAttributes(interfaceMethod);
                var controllerMethodAttributes = GetControllerAttributes(controllerMethod);
                foreach (var virtualControllerMethodAttribute in interfaceMethodAttributes.Concat(controllerMethodAttributes))
                {
                    if (_virtualControllerAttributeBuilder.TryBuild(virtualControllerMethodAttribute) is { } attributeBuilder)
                        methodBuilder.SetCustomAttribute(attributeBuilder);
                }
            }

            return new VirtualControllerInfo(typeBuilder.CreateTypeInfo()!, nclientController.ControllerType);
        }

        private Attribute[] GetInterfaceAttributes(ICustomAttributeProvider attributeProvider)
        {
            var attributes = attributeProvider.GetCustomAttributes(inherit: true).Cast<Attribute>();
            return attributes
                .Select(x => _attributeMapper.TryMap(x))
                .ToArray()!;
        }
        
        private Attribute[] GetControllerAttributes(ICustomAttributeProvider attributeProvider)
        {
            var attributes = attributeProvider.GetCustomAttributes(inherit: true).Cast<Attribute>();
            return attributes
                .Where(x => x?.GetType() != typeof(ControllerAttribute))
                .ToArray()!;
        }

        private MethodBuilder DefineMethod(TypeBuilder typeBuilder, MethodInfo interfaceMethod, MethodInfo controllerMethod)
        {
            var interfaceMethodParams = interfaceMethod.GetParameters();
            var controllerMethodParams = controllerMethod.GetParameters();
            var methodBuilder = typeBuilder.DefineMethod(
                interfaceMethod.Name,
                attributes: MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                interfaceMethod.ReturnType,
                interfaceMethodParams.Select(x => x.ParameterType).ToArray());

            var virtualControllerMethodParams = interfaceMethodParams
                .Zip(controllerMethodParams, (x, y) => (InterfaceMethodParams: x, ControllerMethodParams: y))
                .Select((x, i) => (Param: x, Index: i))
                .ToArray();
            
            foreach (var (paramPair, index) in virtualControllerMethodParams)
            {
                var (interfaceMethodParam, controllerMethodParam) = paramPair;
                var paramBuilder = methodBuilder.DefineParameter(index + 1, interfaceMethodParam.Attributes, interfaceMethodParam.Name);
                
                var interfaceMethodParamAttributes = GetInterfaceAttributes(interfaceMethodParam);
                var controllerMethodParamAttributes = GetControllerAttributes(controllerMethodParam);
                foreach (var virtualControllerParamAttribute in interfaceMethodParamAttributes.Concat(controllerMethodParamAttributes))
                {
                    if (_virtualControllerAttributeBuilder.TryBuild(virtualControllerParamAttribute) is { } attributeBuilder)
                        paramBuilder.SetCustomAttribute(attributeBuilder);
                }
            }

            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ret);

            return methodBuilder;
        }
    }
}
