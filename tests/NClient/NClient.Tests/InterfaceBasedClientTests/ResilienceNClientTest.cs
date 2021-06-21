using System;
using FluentAssertions;
using NClient.Abstractions.Resilience;
using NClient.Exceptions;
using NClient.Providers.Resilience.Polly;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Tests.Clients;
using NUnit.Framework;
using Polly;

namespace NClient.Tests.InterfaceBasedClientTests
{
    [Parallelizable]
    public class ResilienceNClientTest
    {
        private ReturnApiMockFactory _returnApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _returnApiMockFactory = new ReturnApiMockFactory(port: 5014);
        }

        [Test]
        public void Get_InternalServerError_ThrowClientRequestException()
        {
            using var api = _returnApiMockFactory.MockInternalServerError();
            var returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .Build();

            returnClient.Invoking(x => x.Get(1))
                .Should()
                .Throw<ClientRequestException>();
        }

        [Test]
        public void AsResilientInvoke_InternalServerError_NotThrow()
        {
            using var api = _returnApiMockFactory.MockInternalServerError();
            var noOpPolicy = new PollyResiliencePolicyProvider(Policy.NoOpAsync<ResponseContext>());
            var returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .Build();

            returnClient.Invoking(x => x.AsResilient().Invoke(client => client.Get(1), noOpPolicy))
                .Should()
                .NotThrow();
        }

        [Test]
        public void WithResiliencePolicy_InternalServerError_NotThrow()
        {
            using var api = _returnApiMockFactory.MockInternalServerError();
            var returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .WithResiliencePolicy(x => (Func<int, BasicEntity>)x.Get, Policy.NoOpAsync<ResponseContext>())
                .Build();

            returnClient.Invoking(x => x.Get(1))
                .Should()
                .NotThrow();
        }
    }
}
