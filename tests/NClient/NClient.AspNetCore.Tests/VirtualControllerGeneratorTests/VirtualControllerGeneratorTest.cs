using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NClient.Annotations;
using NClient.Annotations.Auth;
using NClient.Annotations.Http;
using NClient.AspNetCore.Controllers;
using NClient.AspNetCore.Controllers.Models;
using NClient.AspNetCore.Exceptions.Factories;
using NClient.AspNetCore.Mappers;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NUnit.Framework;

namespace NClient.AspNetCore.Tests.VirtualControllerGeneratorTests
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadEmptyBracesLineBreaks")]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    [SuppressMessage("ReSharper", "MultipleTypeMembersOnOneLine")]
    public class VirtualControllerGeneratorTest
    {
        private VirtualControllerGenerator _virtualControllerGenerator = null!;
        private IControllerValidationExceptionFactory _controllerValidationExceptionFactory = null!;

        [SetUp]
        public void SetUp()
        {
            _controllerValidationExceptionFactory = new ControllerValidationExceptionFactory();
            _virtualControllerGenerator = new VirtualControllerGenerator(
                new VirtualControllerAttributeBuilder(
                    new ObjectMemberManager(new ControllerObjectMemberManagerExceptionFactory()),
                    new ControllerValidationExceptionFactory()),
                new NClientAttributeMapper(),
                new ControllerValidationExceptionFactory(),
                new GuidProvider());
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
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute() });
        }

        [HttpFacade] public interface IInterfaceWithApiAttribute { }
        public class InterfaceWithApiAttribute : IInterfaceWithApiAttribute { }

        [Test]
        public void Create_InterfaceWithApiAttribute_ApiControllerAttribute()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IInterfaceWithApiAttribute), typeof(InterfaceWithApiAttribute))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(2);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute(), new ApiControllerAttribute() });
        }

        [Authorized] public interface IInterfaceWithAuthorizedAttribute { }
        public class InterfaceWithAuthorizedAttribute : IInterfaceWithAuthorizedAttribute { }

        [Test]
        public void Create_InterfaceWithAuthorizedAttribute_AuthorizedAttribute()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IInterfaceWithAuthorizedAttribute), typeof(InterfaceWithAuthorizedAttribute))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(2);
            controllerAttributes.Should().BeEquivalentTo(new Attribute[] { new ControllerAttribute(), new AuthorizeAttribute() });
        }

        [Anonymous] public interface IInterfaceWithAnonymousAttribute { }
        public class InterfaceWithAnonymousAttribute : IInterfaceWithAnonymousAttribute { }

        [Test]
        public void Create_InterfaceWithAnonymousAttribute_AnonymousAttribute()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IInterfaceWithAnonymousAttribute), typeof(InterfaceWithAnonymousAttribute))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(2);
            controllerAttributes.Should().BeEquivalentTo(new Attribute[] { new ControllerAttribute(), new AllowAnonymousAttribute() });
        }

        [Version("1.0")] public interface IInterfaceWithVersionAttribute { }
        public class InterfaceWithVersionAttribute : IInterfaceWithVersionAttribute { }

        [Test]
        public void Create_InterfaceWithVersionAttribute_ApiVersionAttribute()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IInterfaceWithVersionAttribute), typeof(InterfaceWithVersionAttribute))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(2);
            controllerAttributes.Should().BeEquivalentTo(new Attribute[] { new ControllerAttribute(), new ApiVersionAttribute("1.0") });
        }

        public interface IInterfaceWithToVersionAttribute {[ToVersion("1.0")] void Method(); }
        public class InterfaceWithToVersionAttribute : IInterfaceWithToVersionAttribute { public void Method() { } }

        [Test]
        public void Create_InterfaceWithToVersionAttribute_MapToApiVersionAttribute()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IInterfaceWithToVersionAttribute), typeof(InterfaceWithToVersionAttribute))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute() });
            var methodInfo = virtualControllerType.GetMethod(nameof(InterfaceWithToVersionAttribute.Method))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: true);
            methodAttributes.Length.Should().Be(1);
            methodAttributes.Should().BeEquivalentTo(new[] { new MapToApiVersionAttribute("1.0") });
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
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(2);
            controllerAttributes.Should().BeEquivalentTo(new Attribute[]
            {
                new ControllerAttribute(), new RouteAttribute("api/[controller]") { Order = 0 }
            });
        }

        [HttpFacade, Path("api/[controller]")] public interface IInterfaceWithApiAndPathAttributes { }
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
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(3);
            controllerAttributes.Should().BeEquivalentTo(new Attribute[]
            {
                new ControllerAttribute(), new ApiControllerAttribute(), new RouteAttribute("api/[controller]") { Order = 0 }
            });
        }

        public interface IMethodAttributeController { [GetMethod] int Get(); }
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
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute() });
            var methodInfo = virtualControllerType.GetMethod(nameof(MethodAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: true);
            methodAttributes.Length.Should().Be(1);
            methodAttributes.Should().BeEquivalentTo(new[] { new HttpGetAttribute { Order = 0 } }, 
                opts => opts.ComparingByMembers<HttpGetAttribute>());
        }
        
        public interface IInterfaceAspNetMethodAttributeController { [HttpGet] int Get(); }
        public class InterfaceAspNetMethodAttributeController : IInterfaceAspNetMethodAttributeController { public int Get() => 1; }

        [Test]
        public void Create_InterfaceAspNetMethodAttributeController_AddMethodAttribute()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IInterfaceAspNetMethodAttributeController), typeof(InterfaceAspNetMethodAttributeController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute() });
            var methodInfo = virtualControllerType.GetMethod(nameof(MethodAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: true);
            methodAttributes.Length.Should().Be(1);
            methodAttributes.Should().BeEquivalentTo(new[] { new HttpGetAttribute { Order = 0 } }, 
                opts => opts.ComparingByMembers<HttpGetAttribute>());
        }
        
        public interface IControllerAspNetMethodAttributeController { int Get(); }
        public class ControllerAspNetMethodAttributeController : IControllerAspNetMethodAttributeController { [HttpGet] public int Get() => 1; }

        [Test]
        public void Create_ControllerAspNetMethodAttributeController_AddMethodAttribute()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IControllerAspNetMethodAttributeController), typeof(ControllerAspNetMethodAttributeController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute() });
            var methodInfo = virtualControllerType.GetMethod(nameof(MethodAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: true);
            methodAttributes.Length.Should().Be(1);
            methodAttributes.Should().BeEquivalentTo(new[] { new HttpGetAttribute { Order = 0 } }, 
                opts => opts.ComparingByMembers<HttpGetAttribute>());
        }

        public interface IMethodAttributeWithTemplateController { [GetMethod("[action]")] int Get(); }
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
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute() });
            var methodInfo = virtualControllerType.GetMethod(nameof(MethodAttributeWithTemplateController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: true);
            methodAttributes.Length.Should().Be(1);
            methodAttributes.Should().BeEquivalentTo(new[] { new HttpGetAttribute("[action]") { Order = 0 } }, 
                opts => opts.ComparingByMembers<HttpGetAttribute>());
        }

        public interface IMultipleMethodController { [GetMethod] int Get(); [PostMethod] int Post(); }
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
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute() });
            var getMethodInfo = virtualControllerType.GetMethod(nameof(MultipleMethodController.Get))!;
            var getMethodAttributes = getMethodInfo.GetCustomAttributes(inherit: true);
            getMethodAttributes.Length.Should().Be(1);
            getMethodAttributes.Should().BeEquivalentTo(new[] { new HttpGetAttribute { Order = 0 } }, 
                opts => opts.ComparingByMembers<HttpGetAttribute>());
            var postMethodInfo = virtualControllerType.GetMethod(nameof(MultipleMethodController.Post))!;
            var postMethodAttributes = postMethodInfo.GetCustomAttributes(inherit: true);
            postMethodAttributes.Length.Should().Be(1);
            postMethodAttributes.Should().BeEquivalentTo(new[] { new HttpPostAttribute { Order = 0 } }, 
                opts => opts.ComparingByMembers<HttpPostAttribute>());
        }

        public class CustomAttribute : Attribute { }
        public interface INotNClientMethodAttributeController { [Custom] int Get(); }
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
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute() });
            var methodInfo = virtualControllerType.GetMethod(nameof(NotNClientMethodAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: true);
            methodAttributes.Length.Should().Be(1);
            methodAttributes.Should().BeEquivalentTo(new[] { new CustomAttribute() });
        }

        public interface IResponseAttributeController { [GetMethod, Response(typeof(int), HttpStatusCode.OK)] int Get(); }
        public class ResponseAttributeController : IResponseAttributeController { public int Get() => 1; }

        [Test]
        public void Create_ResponseAttributeController_AddResponseAttribute()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IResponseAttributeController), typeof(ResponseAttributeController))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute() });
            var methodInfo = virtualControllerType.GetMethod(nameof(ResponseAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: true);
            methodAttributes.Length.Should().Be(2);
            methodAttributes[0].Should().BeEquivalentTo(new HttpGetAttribute { Order = 0 }, opts => opts.ComparingByMembers<HttpGetAttribute>());
            methodAttributes[1].Should().BeEquivalentTo(new ProducesResponseTypeAttribute(typeof(int), 200), opts => opts.ComparingByMembers<ProducesResponseTypeAttribute>());
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
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute() });
            var methodInfo = virtualControllerType.GetMethod(nameof(ParameterAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: true);
            methodAttributes.Length.Should().Be(0);
            var methodParams = methodInfo.GetParameters();
            methodParams.Length.Should().Be(1);
            var methodParamAttributes = methodParams[0].GetCustomAttributes(inherit: true);
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
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute() });
            var methodInfo = virtualControllerType.GetMethod(nameof(ParameterAttributeWithNameController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: true);
            methodAttributes.Length.Should().Be(0);
            var methodParams = methodInfo.GetParameters();
            methodParams.Length.Should().Be(1);
            var methodParamAttributes = methodParams[0].GetCustomAttributes(inherit: true);
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
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute() });
            var methodInfo = virtualControllerType.GetMethod(nameof(ParameterAttributeController.Get))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: true);
            methodAttributes.Length.Should().Be(0);
            var methodParams = methodInfo.GetParameters();
            methodParams.Length.Should().Be(2);
            var idParamAttributes = methodParams[0].GetCustomAttributes(inherit: true);
            idParamAttributes.Length.Should().Be(1);
            idParamAttributes[0].Should().BeEquivalentTo(new FromQueryAttribute());
            var nameParamAttributes = methodParams[1].GetCustomAttributes(inherit: true);
            nameParamAttributes.Length.Should().Be(1);
            nameParamAttributes[0].Should().BeEquivalentTo(new FromBodyAttribute());
        }
        
        public interface IGenericInterface<T> { T Method(); }
        public class GenericInterface : IGenericInterface<int> { public int Method() { return 1; } }

        [Test]
        public void Create_GenericInterface_MapToGenericController()
        {
            var nclientControllers = new[]
            {
                new NClientControllerInfo(typeof(IGenericInterface<int>), typeof(GenericInterface))
            };

            var actualResult = _virtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();

            actualResult.Should().ContainSingle();
            var virtualControllerType = actualResult.Single().Type;
            var controllerAttributes = virtualControllerType.GetCustomAttributes(inherit: true);
            controllerAttributes.Length.Should().Be(1);
            controllerAttributes.Should().BeEquivalentTo(new[] { new ControllerAttribute() });
            var methodInfo = virtualControllerType.GetMethod(nameof(GenericInterface.Method))!;
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: true);
            methodAttributes.Should().BeEmpty();
        }
    }
}
