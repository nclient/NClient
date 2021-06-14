using System.Threading.Tasks;
using FluentAssertions;
using NClient.Core.Resilience;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Tests.Clients;
using NUnit.Framework;

namespace NClient.Tests.InterfaceBasedClientTests
{
    [Parallelizable]
    public class ResilienceNClientTest
    {
        private IReturnClientWithMetadata _returnClient = null!;
        private ReturnApiMockFactory _returnApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _returnApiMockFactory = new ReturnApiMockFactory(port: 5014);
            _returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task InvokeResiliently_GetAsync_BasicEntity()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = await _returnClient.AsResilient().Invoke(client => client.GetAsync(id), new StubResiliencePolicyProvider());

            result.Should().BeEquivalentTo(entity);
        }

        [Test]
        public void InvokeResiliently_Get_BasicEntity()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetMethod(id, entity);

            var result = _returnClient.AsResilient().Invoke(client => client.Get(id), new StubResiliencePolicyProvider());

            result.Should().BeEquivalentTo(entity);
        }
    }
}
