using System;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.AspNetCore.Controllers;
using NClient.AspNetCore.Controllers.Models;
using NClient.AspNetCore.Mappers;
using NClient.Core.Exceptions;
using NClient.Core.Helpers;
using NUnit.Framework;

namespace NClient.AspNetCore.Tests.VirtualControllerGeneratorTests
{
    public class VirtualControllerGeneratorTest
    {
        private VirtualControllerGenerator _virtualControllerGenerator = null!;

        [SetUp]
        public void SetUp()
        {
            var attributeMapper = new NClientAttributeMapper();
            var guidProvider = new GuidProvider();
            _virtualControllerGenerator = new VirtualControllerGenerator(attributeMapper, guidProvider);
        }

        public interface IInterfaceWithoutAttributes { }
        public class InterfaceWithoutAttributes : IInterfaceWithoutAttributes { }

        [Test]
        public void Create_InterfaceWithoutAttributes_NoAttributes()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IInterfaceWithoutAttributes), typeof(InterfaceWithoutAttributes))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(0);
        }

        [Api] public interface IInterfaceWithoutApiAttribute { }
        public class InterfaceWithoutApiAttribute : IInterfaceWithoutApiAttribute { }

        [Test]
        public void Create_InterfaceWithoutApiAttribute_ApiControllerAttribute()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IInterfaceWithoutApiAttribute), typeof(InterfaceWithoutApiAttribute))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes[0].Should().BeEquivalentTo(new ApiControllerAttribute());
        }

        [Path("api/[controller]")] public interface IInterfaceWithPathAttributeWithTemplate { }
        public class InterfaceWithPathAttributeWithTemplate : IInterfaceWithPathAttributeWithTemplate { }

        [Test]
        public void Create_InterfaceWithPathAttributeWithTemplate_AddApiControllerAndRouteWithTemplateAttributes()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IInterfaceWithPathAttributeWithTemplate), typeof(InterfaceWithPathAttributeWithTemplate))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes[0].Should().BeEquivalentTo(new RouteAttribute("api/[controller]") { Order = 0 });
        }

        [Api, Path("api/[controller]")] public interface IInterfaceWithApiAndPathAttributes { }
        public class InterfaceWithApiAndPathAttributes : IInterfaceWithApiAndPathAttributes { }

        [Test]
        public void Create_InterfaceWithApiAndPathAttributes_AddApiControllerAndRouteWithTemplateAttributes()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IInterfaceWithApiAndPathAttributes), typeof(InterfaceWithApiAndPathAttributes))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(2);
            controllerAttributes[0].Should().BeEquivalentTo(new ApiControllerAttribute());
            controllerAttributes[1].Should().BeEquivalentTo(new RouteAttribute("api/[controller]") { Order = 0 });
        }

        public interface IMethodAttributeController {[GetMethod] int Get(); }
        public class MethodAttributeController : IMethodAttributeController { public int Get() => 1; }

        [Test]
        public void Create_MethodAttributeController_AddMethodAttribute()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IMethodAttributeController), typeof(MethodAttributeController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(0);
            var methodInfo = virtualControllerType.GetMethod(nameof(MethodAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: false);
            methodAttributes.Length.Should().Be(1);
            methodAttributes.Should().BeEquivalentTo(new HttpGetAttribute { Order = 0 });
        }

        public interface IMethodAttributeWithTemplateController {[GetMethod("[action]")] int Get(); }
        public class MethodAttributeWithTemplateController : IMethodAttributeWithTemplateController { public int Get() => 1; }

        [Test]
        public void Create_MethodAttributeWithTemplateController_AddMethodAttributeWithTemplate()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IMethodAttributeWithTemplateController), typeof(MethodAttributeWithTemplateController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(0);
            var methodInfo = virtualControllerType.GetMethod(nameof(MethodAttributeWithTemplateController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: false);
            methodAttributes.Length.Should().Be(1);
            methodAttributes.Should().BeEquivalentTo(new HttpGetAttribute("[action]") { Order = 0 });
        }

        public interface IMultipleMethodController {[GetMethod] int Get();[PostMethod] int Post(); }
        public class MultipleMethodController : IMultipleMethodController { public int Get() => 1; public int Post() => 1; }

        [Test]
        public void Create_MultipleMethodController_AddMethodAttributeForEachMethod()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IMultipleMethodController), typeof(MultipleMethodController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(0);
            var getMethodInfo = virtualControllerType.GetMethod(nameof(MultipleMethodController.Get))!;
            var getMethodAttributes = getMethodInfo.GetCustomAttributes(inherit: false);
            getMethodAttributes.Length.Should().Be(1);
            getMethodAttributes.Should().BeEquivalentTo(new HttpGetAttribute { Order = 0 });
            var postMethodInfo = virtualControllerType.GetMethod(nameof(MultipleMethodController.Post))!;
            var postMethodAttributes = postMethodInfo.GetCustomAttributes(inherit: false);
            postMethodAttributes.Length.Should().Be(1);
            postMethodAttributes.Should().BeEquivalentTo(new HttpPostAttribute { Order = 0 });
        }

        public interface IAspNetMethodAttributeController {[HttpGet] int Get(); }
        public class AspNetMethodAttributeController : IAspNetMethodAttributeController { public int Get() => 1; }

        [Test]
        public void Create_AspNetMethodAttributeController_ThrowInvalidAttributeNClientException()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IAspNetMethodAttributeController), typeof(AspNetMethodAttributeController))
            };

            _virtualControllerGenerator
                .Invoking(x => x.Create(nclientControllers).ToArray())
                .Should()
                .ThrowExactly<InvalidAttributeNClientException>();
        }

        public class CustomAttribute : Attribute { }
        public interface INotNClientMethodAttributeController {[Custom] int Get(); }
        public class NotNClientMethodAttributeController : INotNClientMethodAttributeController { public int Get() => 1; }

        [Test]
        public void Create_NotNClientMethodAttributeController_IgnoreCustomAttribute()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(INotNClientMethodAttributeController), typeof(NotNClientMethodAttributeController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(0);
            var methodInfo = virtualControllerType.GetMethod(nameof(NotNClientMethodAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: false);
            methodAttributes.Length.Should().Be(0);
        }

        public interface IParameterAttributeController { int Get([QueryParam] int id); }
        public class ParameterAttributeController : IParameterAttributeController { public int Get(int id) => 1; }

        [Test]
        public void Create_ParameterAttributeController_AddParameterAttribute()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IParameterAttributeController), typeof(ParameterAttributeController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(0);
            var methodInfo = virtualControllerType.GetMethod(nameof(ParameterAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: false);
            methodAttributes.Length.Should().Be(0);
            var methodParams = methodInfo.GetParameters();
            methodParams.Length.Should().Be(1);
            var methodParamAttributes = methodParams[0].GetCustomAttributes(inherit: false);
            methodParamAttributes.Length.Should().Be(1);
            methodParamAttributes[0].Should().BeEquivalentTo(new FromQueryAttribute());
        }

        public interface IParameterAttributeWithNameController { int Get([QueryParam(Name = "myId")] int id); }
        public class ParameterAttributeWithNameController : IParameterAttributeWithNameController { public int Get(int id) => 1; }

        [Test]
        public void Create_ParameterAttributeWithNameController_AddParameterAttributeWithName()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IParameterAttributeWithNameController), typeof(ParameterAttributeWithNameController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(0);
            var methodInfo = virtualControllerType.GetMethod(nameof(ParameterAttributeWithNameController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: false);
            methodAttributes.Length.Should().Be(0);
            var methodParams = methodInfo.GetParameters();
            methodParams.Length.Should().Be(1);
            var methodParamAttributes = methodParams[0].GetCustomAttributes(inherit: false);
            methodParamAttributes.Length.Should().Be(1);
            methodParamAttributes[0].Should().BeEquivalentTo(new FromQueryAttribute { Name = "myId" });
        }

        public interface IMultipleParameterAttributeController { int Get([QueryParam] int id, [BodyParam] string name); }
        public class MultipleParameterAttributeController : IMultipleParameterAttributeController { public int Get(int id, string name) => 1; }

        [Test]
        public void Create_MultipleParameterAttributeController_AddParameterAttributeForEachParameter()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IMultipleParameterAttributeController), typeof(MultipleParameterAttributeController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: false);
            controllerAttributes.Length.Should().Be(0);
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
