﻿using System.Threading.Tasks;
using FluentAssertions;
using NClient.Core.Resilience;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NClient.Tests.Controllers;
using NUnit.Framework;

#pragma warning disable 618

namespace NClient.Tests.ControllerBasedClientTests
{
    [Parallelizable]
    public class ResilienceNClientTest
    {
        private IReturnClient _returnClient = null!;
        private ReturnApiMockFactory _returnApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _returnApiMockFactory = new ReturnApiMockFactory(port: 5016);
            _returnClient = new NClientBuilder()
                .Use<IReturnClient, ReturnController>(_returnApiMockFactory.ApiUri.ToString())
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
