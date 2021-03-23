using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NClient.AspNetProxy.Extensions;
using NClient.Core.Attributes;
using NClient.Core.Attributes.Methods;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.AspNetProxy.Standalone.Tests.AddNClientControllersExtensionsTests
{
    [Parallelizable]
    public class AddNClientControllersExtensionsTest
    {
        [Path("api/[controller]")] public interface IGetController { int Get(int id); }
        public class GetController : ControllerBase, IGetController { public int Get(int id) => id; }

        [Test]
        public void AddNClientControllers_GetController_ReturnInputValue()
        {
            const int id = 1;

            var actualResult = InvokeMethod<IGetController, GetController, int>(
                methodName: nameof(GetController.Get),
                parameters: new object[] { id });

            actualResult.Should().Be(id);
        }

        [Path("api/[controller]")] public interface IGetParameterlessController { int Get(); }
        public class GetParameterlessController : ControllerBase, IGetParameterlessController { public int Get() => 1; }

        [Test]
        public void AddNClientControllers_GetParameterlessController_ReturnValue()
        {
            var actualResult = InvokeMethod<IGetParameterlessController, GetParameterlessController, int>(
                methodName: nameof(GetParameterlessController.Get),
                parameters: new object[] { });

            actualResult.Should().Be(1);
        }

        [Path("api/[controller]")] public interface IGetWithMultipleParamsController { int Get(int id, string name); }
        public class GetWithMultipleParamsController : ControllerBase, IGetWithMultipleParamsController { public int Get(int id, string name) => id; }

        [Test]
        public void AddNClientControllers_GetWithMultipleParamsController_ReturnInputValue()
        {
            const int id = 1;

            var actualResult = InvokeMethod<IGetWithMultipleParamsController, GetWithMultipleParamsController, int>(
                methodName: nameof(GetWithMultipleParamsController.Get),
                parameters: new object[] { id, "name" });

            actualResult.Should().Be(id);
        }

        [Path("api/[controller]")] public interface IGetAsyncController { Task<int> GetAsync(int id); }
        public class GetAsyncController : ControllerBase, IGetAsyncController { public Task<int> GetAsync(int id) => Task.FromResult(1); }

        [Test]
        public async Task AddNClientControllers_GetAsyncController_ReturnValue()
        {
            const int id = 1;

            var actualResult = await InvokeMethod<IGetAsyncController, GetAsyncController, Task<int>>(
                methodName: nameof(GetAsyncController.GetAsync),
                parameters: new object[] { id })!;

            actualResult.Should().Be(id);
        }

        [Path("api/[controller]")] public interface IPostController { [PostMethod] void Post(BasicEntity entity); }
        public class PostController : ControllerBase, IPostController { public void Post(BasicEntity entity) { } }

        [Test]
        public void AddNClientControllers_PostController_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };

            var actualResult = InvokeMethod<IPostController, PostController, object>(
                methodName: nameof(PostAndReturnController.Post),
                parameters: new object[] { entity });

            actualResult.Should().BeNull();
        }

        [Path("api/[controller]")] public interface IPostAndReturnController { [PostMethod] BasicEntity Post(BasicEntity entity); }
        public class PostAndReturnController : ControllerBase, IPostAndReturnController { public BasicEntity Post(BasicEntity entity) => entity; }

        [Test]
        public void AddNClientControllers_PostAndReturnController_ReturnInputEntity()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };

            var actualResult = InvokeMethod<IPostAndReturnController, PostAndReturnController, BasicEntity>(
                methodName: nameof(PostAndReturnController.Post),
                parameters: new object[] {entity});
            
            actualResult.Should().BeEquivalentTo(entity);
        }

        private static TResult? InvokeMethod<TInterface, TController, TResult>(string methodName, object[] parameters) 
            where TController : ControllerBase, TInterface
        {
            var services = new ServiceCollection()
                .AddNClientControllers(options => options.AddController<TInterface, TController>());
            var virtualControllerName = $"{NClientAssemblyNames.NClientDynamicControllerProxies}.{typeof(TController).Name}";
            var virtualControllerType = services
                .Single(x => x.ServiceType.FullName == virtualControllerName)
                .ServiceType;
            var serviceProvider = services.BuildServiceProvider();

            var controller = serviceProvider.GetRequiredService(virtualControllerType);
            var actualResult = controller.GetType().GetMethod(methodName)!
                .Invoke(controller, parameters);

            return (TResult)actualResult;
        }
    }
}
