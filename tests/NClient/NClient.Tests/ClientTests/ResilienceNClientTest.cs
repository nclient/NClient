using System;
using System.Net.Http;
using FluentAssertions;
using FluentAssertions.Extensions;
using NClient.Abstractions.Resilience;
using NClient.Exceptions;
using NClient.Providers.Resilience.Polly;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Tests.Clients;
using NUnit.Framework;
using Polly;

namespace NClient.Tests.ClientTests
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
                .ThrowExactly<ClientRequestException>();
        }

        [Test]
        public void AsResilientInvoke_InternalServerError_NotThrow()
        {
            using var api = _returnApiMockFactory.MockInternalServerError();
            var noOpPolicy = new PollyResiliencePolicyProvider<HttpRequestMessage, HttpResponseMessage>(Policy.NoOpAsync<ResponseContext<HttpRequestMessage, HttpResponseMessage>>());
            var returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .Build();

            returnClient.Invoking(x => x.AsResilient().Invoke(client => client.Get(1), noOpPolicy))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithResiliencePolicy_GetRequestWithFlakyInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = _returnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity { Id = id });
            var returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .WithResiliencePolicy()
                .Build();

            returnClient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithResiliencePolicy_PostRequestWithFlakyInternalServerError_NotThrow()
        {
            var entity = new BasicEntity { Id = 1 };
            using var api = _returnApiMockFactory.MockFlakyPostMethod(entity);
            api.AllowPartialMapping();
            var returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .WithResiliencePolicy()
                .Build();

            returnClient.Invoking(x => x.Post(entity))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithResiliencePolicy_GetHttpResponseWithInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = _returnApiMockFactory.MockInternalServerError();
            var returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .WithResiliencePolicy()
                .Build();

            returnClient.Invoking(x => x.GetHttpResponse(id))
                .Should()
                .NotThrow();
        }

        [Test]
        public void WithResiliencePolicyForGet_GetRequestWithInternalServerError_NotThrow()
        {
            using var api = _returnApiMockFactory.MockInternalServerError();
            var returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .WithResiliencePolicy(x => (Func<int, BasicEntity>)x.Get, Policy.NoOpAsync<ResponseContext<HttpRequestMessage, HttpResponseMessage>>())
                .Build();

            returnClient.Invoking(x => x.Get(1))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithResiliencePolicyForGet_PostRequestWithInternalServerError_ThrowClientRequestException()
        {
            var entity = new BasicEntity { Id = 1 };
            using var api = _returnApiMockFactory.MockInternalServerError();
            var returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .WithResiliencePolicy(x => (Func<int, BasicEntity>)x.Get, Policy.NoOpAsync<ResponseContext<HttpRequestMessage, HttpResponseMessage>>())
                .Build();

            returnClient.Invoking(x => x.Post(entity))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }

        [Test]
        public void WithResiliencePolicyForSafeMethods_GetRequestWithInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = _returnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity { Id = id });
            api.AllowPartialMapping();
            var returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .WithResiliencePolicyForSafeMethods(sleepDurationProvider: _ => 0.Seconds())
                .Build();

            returnClient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }

        [Test]
        public void WithResiliencePolicyForSafeMethods_PostRequestWithInternalServerError_ThrowClientRequestException()
        {
            var entity = new BasicEntity { Id = 1 };
            using var api = _returnApiMockFactory.MockFlakyPostMethod(entity);
            api.AllowPartialMapping();
            var returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .WithResiliencePolicyForSafeMethods(sleepDurationProvider: _ => 0.Seconds())
                .Build();

            returnClient.Invoking(x => x.Post(entity))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }

        [Test]
        public void WithResiliencePolicyForIdempotentMethods_GetRequestWithInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = _returnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity { Id = id });
            api.AllowPartialMapping();
            var returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .WithResiliencePolicyForIdempotentMethods(sleepDurationProvider: _ => 0.Seconds())
                .Build();

            returnClient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }

        [Test]
        public void WithResiliencePolicyForIdempotentMethods_PostRequestWithInternalServerError_ThrowClientRequestException()
        {
            var entity = new BasicEntity { Id = 1 };
            using var api = _returnApiMockFactory.MockFlakyPostMethod(entity);
            api.AllowPartialMapping();
            var returnClient = new NClientBuilder()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .WithResiliencePolicyForIdempotentMethods(sleepDurationProvider: _ => 0.Seconds())
                .Build();

            returnClient.Invoking(x => x.Post(entity))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }
    }
}
