using System.Threading.Tasks;
using FluentAssertions;
using NClient.AspNetProxy.Extensions;
using NClient.AspNetProxy.Tests.Controllers;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.AspNetProxy.Tests.ClientTests
{
    [Parallelizable]
    public class ReturnControllerTest
    {
        private IReturnClient _returnClient = null!;
        private ReturnApiMockFactory _returnApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _returnApiMockFactory = new ReturnApiMockFactory(port: 5005);
            _returnClient = new ControllerClientProvider()
                .Use<IReturnClient, ReturnController>(_returnApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task ReturnClient_GetAsync_HttpResponseWithValue()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = await _returnClient.GetAsync(id);
            result.Should().BeEquivalentTo(entity);
        }

        [Test]
        public void ReturnClient_Get_HttpResponseWithValue()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetMethod(id, entity);

            var result = _returnClient.Get(id);
            result.Should().BeEquivalentTo(entity);
        }

        [Test]
        public async Task ReturnClient_PostAsync_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostAsyncMethod(entity);

            await _returnClient.PostAsync(entity);
        }

        [Test]
        public void ReturnClient_Post_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostMethod(entity);

            _returnClient.Post(entity);
        }
    }
}
