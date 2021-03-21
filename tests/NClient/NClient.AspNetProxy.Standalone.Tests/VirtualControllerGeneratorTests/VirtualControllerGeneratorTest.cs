using System;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NClient.AspNetProxy.Attributes;
using NClient.AspNetProxy.Controllers;
using NClient.Core.Attributes.Clients;
using NClient.Core.Attributes.Clients.Methods;
using NClient.Core.Attributes.Clients.Parameters;
using NClient.Core.Exceptions;
using NUnit.Framework;

namespace NClient.AspNetProxy.Standalone.Tests.VirtualControllerGeneratorTests
{
    public class VirtualControllerGeneratorTest
    {
        private VirtualControllerGenerator _virtualControllerGenerator = null!;

        [SetUp]
        public void SetUp()
        {
            var attributeMapper = new NClientAttributeMapper();
            _virtualControllerGenerator = new VirtualControllerGenerator(attributeMapper);
        }

        [Client] public interface IInterfaceAttributeController { }
        public class InterfaceAttributeController : IInterfaceAttributeController { }

        [Test]
        public void Create_InterfaceAttribute_AddApiControllerAndRouteAttributes()
        {
            var interfaceControllerPairs = new[]
            {
                (typeof(IInterfaceAttributeController), typeof(InterfaceAttributeController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(interfaceControllerPairs)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().VirtualControllerType;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(2);
            controllerAttributes[0].Should().BeEquivalentTo(new ApiControllerAttribute());
            controllerAttributes[1].Should().BeEquivalentTo(new RouteAttribute("") { Order = 0 });
        }

        [Client("api/[controller]")] public interface IInterfaceAttributeWithTemplateController { }
        public class InterfaceAttributeWithTemplateController : IInterfaceAttributeWithTemplateController { }

        [Test]
        public void Create_InterfaceAttributeWithTemplate_AddApiControllerAndRouteWithTemplateAttributes()
        {
            var interfaceControllerPairs = new[]
            {
                (typeof(IInterfaceAttributeWithTemplateController), typeof(InterfaceAttributeWithTemplateController))
            };
            
            var actualResult = _virtualControllerGenerator
                .Create(interfaceControllerPairs)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().VirtualControllerType;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(2);
            controllerAttributes[0].Should().BeEquivalentTo(new ApiControllerAttribute());
            controllerAttributes[1].Should().BeEquivalentTo(new RouteAttribute("api/[controller]") { Order = 0 });
        }

        public interface IMethodAttributeController { [AsHttpGet] int Get(); }
        public class MethodAttributeController : IMethodAttributeController { public int Get() => 1; }

        [Test]
        public void Create_MethodAttributeController_AddMethodAttribute()
        {
            var interfaceControllerPairs = new[]
            {
                (typeof(IMethodAttributeController), typeof(MethodAttributeController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(interfaceControllerPairs)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().VirtualControllerType;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes[0].Should().BeEquivalentTo(new ApiControllerAttribute());
            var methodInfo = virtualControllerType.GetMethod(nameof(MethodAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: false);
            methodAttributes.Length.Should().Be(1);
            methodAttributes.Should().BeEquivalentTo(new HttpGetAttribute("") { Order = 0 });
        }

        public interface IMethodAttributeWithTemplateController { [AsHttpGet("[action]")] int Get(); }
        public class MethodAttributeWithTemplateController : IMethodAttributeWithTemplateController { public int Get() => 1; }

        [Test]
        public void Create_MethodAttributeWithTemplateController_AddMethodAttributeWithTemplate()
        {
            var interfaceControllerPairs = new[]
            {
                (typeof(IMethodAttributeWithTemplateController), typeof(MethodAttributeWithTemplateController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(interfaceControllerPairs)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().VirtualControllerType;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes[0].Should().BeEquivalentTo(new ApiControllerAttribute());
            var methodInfo = virtualControllerType.GetMethod(nameof(MethodAttributeWithTemplateController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: false);
            methodAttributes.Length.Should().Be(1);
            methodAttributes.Should().BeEquivalentTo(new HttpGetAttribute("[action]") { Order = 0 });
        }

        public interface IMultipleMethodController { [AsHttpGet] int Get(); [AsHttpPost] int Post(); }
        public class MultipleMethodController : IMultipleMethodController { public int Get() => 1; public int Post() => 1; }

        [Test]
        public void Create_MultipleMethodController_AddMethodAttributeForEachMethod()
        {
            var interfaceControllerPairs = new[]
            {
                (typeof(IMultipleMethodController), typeof(MultipleMethodController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(interfaceControllerPairs)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().VirtualControllerType;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes[0].Should().BeEquivalentTo(new ApiControllerAttribute());
            var getMethodInfo = virtualControllerType.GetMethod(nameof(MultipleMethodController.Get))!;
            var getMethodAttributes = getMethodInfo.GetCustomAttributes(inherit: false);
            getMethodAttributes.Length.Should().Be(1);
            getMethodAttributes.Should().BeEquivalentTo(new HttpGetAttribute("") { Order = 0 });
            var postMethodInfo = virtualControllerType.GetMethod(nameof(MultipleMethodController.Post))!;
            var postMethodAttributes = postMethodInfo.GetCustomAttributes(inherit: false);
            postMethodAttributes.Length.Should().Be(1);
            postMethodAttributes.Should().BeEquivalentTo(new HttpPostAttribute("") { Order = 0 });
        }

        public interface IAspNetMethodAttributeController {[HttpGet] int Get(); }
        public class AspNetMethodAttributeController : IAspNetMethodAttributeController { public int Get() => 1; }

        [Test]
        public void Create_AspNetMethodAttributeController_ThrowInvalidAttributeNClientException()
        {
            var interfaceControllerPairs = new[]
            {
                (typeof(IAspNetMethodAttributeController), typeof(AspNetMethodAttributeController))
            };

            _virtualControllerGenerator
                .Invoking(x => x.Create(interfaceControllerPairs).ToArray())
                .Should()
                .ThrowExactly<InvalidAttributeNClientException>();
        }

        public class CustomAttribute : Attribute { }
        public interface INotNClientMethodAttributeController { [Custom] int Get(); }
        public class NotNClientMethodAttributeController : INotNClientMethodAttributeController { public int Get() => 1; }

        [Test]
        public void Create_NotNClientMethodAttributeController_IgnoreCustomAttribute()
        {
            var interfaceControllerPairs = new[]
            {
                (typeof(INotNClientMethodAttributeController), typeof(NotNClientMethodAttributeController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(interfaceControllerPairs)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().VirtualControllerType;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes[0].Should().BeEquivalentTo(new ApiControllerAttribute());
            var methodInfo = virtualControllerType.GetMethod(nameof(NotNClientMethodAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: false);
            methodAttributes.Length.Should().Be(0);
        }

        public interface IParameterAttributeController { int Get([ToQuery] int id); }
        public class ParameterAttributeController : IParameterAttributeController { public int Get(int id) => 1; }

        [Test]
        public void Create_ParameterAttributeController_AddParameterAttribute()
        {
            var interfaceControllerPairs = new[]
            {
                (typeof(IParameterAttributeController), typeof(ParameterAttributeController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(interfaceControllerPairs)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().VirtualControllerType;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes[0].Should().BeEquivalentTo(new ApiControllerAttribute());
            var methodInfo = virtualControllerType.GetMethod(nameof(ParameterAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: false);
            methodAttributes.Length.Should().Be(0);
            var methodParams = methodInfo.GetParameters();
            methodParams.Length.Should().Be(1);
            var methodParamAttributes = methodParams[0].GetCustomAttributes(inherit: false);
            methodParamAttributes.Length.Should().Be(1);
            methodParamAttributes[0].Should().BeEquivalentTo(new FromQueryAttribute());
        }

        public interface IMultipleParameterAttributeController { int Get([ToQuery] int id, [ToBody] string name); }
        public class MultipleParameterAttributeController : IMultipleParameterAttributeController { public int Get(int id, string name) => 1; }

        [Test]
        public void Create_MultipleParameterAttributeController_AddParameterAttributeForEachParameter()
        {
            var interfaceControllerPairs = new[]
            {
                (typeof(IMultipleParameterAttributeController), typeof(MultipleParameterAttributeController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(interfaceControllerPairs)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().VirtualControllerType;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes[0].Should().BeEquivalentTo(new ApiControllerAttribute());
            var methodInfo = virtualControllerType.GetMethod(nameof(ParameterAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: false);
            methodAttributes.Length.Should().Be(0);
            var methodParams = methodInfo.GetParameters();
            methodParams.Length.Should().Be(2);
            var idParamAttributes = methodParams[0].GetCustomAttributes(inherit: false);
            idParamAttributes.Length.Should().Be(1);
            idParamAttributes[0].Should().BeEquivalentTo(new FromQueryAttribute());
            var nameParamAttributes = methodParams[1].GetCustomAttributes(inherit: false);
            nameParamAttributes.Length.Should().Be(1);
            nameParamAttributes[0].Should().BeEquivalentTo(new FromBodyAttribute());
        }
    }
}
