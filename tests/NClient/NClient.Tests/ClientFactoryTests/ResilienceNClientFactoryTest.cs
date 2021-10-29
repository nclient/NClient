using System;
using System.Net.Http;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using NClient.Exceptions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientFactoryTests
{
    // TODO: remove duplication in ResilienceNClientFactoryTest and ResilienceNClientTest
    [Parallelizable]
    public class ResilienceNClientFactoryTest
    {
        private ReturnApiMockFactory _returnApiMockFactory = null!;
        private INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> _basicReturnClientFactory = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _returnApiMockFactory = new ReturnApiMockFactory(PortsPool.Get());
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            PortsPool.Put(_returnApiMockFactory.ApiUri.Port);
        }
        
        [SetUp]
        public void Setup()
        {
            _basicReturnClientFactory = NClientGallery.ClientFactories
                .GetBasic()
                .For(factoryName: "TestFactory");
        }

        [Test]
        public void WithoutResilience_FlakyInternalServerError_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = _returnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity());
            var returnClientFactory = _basicReturnClientFactory
                .WithoutResilience()
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.Get(id))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }
        
        [Test]
        public void WithoutResilience_FlakyInternalServerError_OverrideFullResilienceAndThrowClientRequestException()
        {
            const int id = 1;
            using var api = _returnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity());
            var returnClientFactory = _basicReturnClientFactory
                .WithFullResilience(getDelay: _ => TimeSpan.FromSeconds(0))
                .WithoutResilience()
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.Get(id))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }

        [Test]
        public void WithFullResilience_GetRequestToFlakyInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = _returnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity { Id = id });
            var returnClientFactory = _basicReturnClientFactory
                .WithFullResilience(getDelay: _ => 0.Seconds())
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithFullResilience_PostRequestToFlakyInternalServerError_NotThrow()
        {
            var entity = new BasicEntity { Id = 1 };
            using var api = _returnApiMockFactory.MockFlakyPostMethod(entity);
            api.AllowPartialMapping();
            var returnClientFactory = _basicReturnClientFactory
                .WithFullResilience(getDelay: _ => 0.Seconds())
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.Post(entity))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithFullResilience_GetIHttpResponseToInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = _returnApiMockFactory.MockInternalServerError();
            var returnClientFactory = _basicReturnClientFactory
                .WithFullResilience(getDelay: _ => 0.Seconds())
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.GetIHttpResponse(id))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithFullResilience_GetHttpResponseMessageToInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = _returnApiMockFactory.MockInternalServerError();
            var returnClientFactory = _basicReturnClientFactory
                .WithFullResilience(getDelay: _ => 0.Seconds())
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.GetHttpResponseMessage(id))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithFullResilience_ExceptPostRequestToFlakyInternalServerError_ThrowClientRequestException()
        {
            var entity = new BasicEntity { Id = 1 };
            using var api = _returnApiMockFactory.MockFlakyPostMethod(entity);
            api.AllowPartialMapping();
            var returnClientFactory = _basicReturnClientFactory
                .WithFullResilience(getDelay: _ => 0.Seconds())
                .WithCustomResilience(x => x
                    .ForMethodOf<IReturnClientWithMetadata>(client => (Action<BasicEntity>)client.Post)
                    .DoNotUse())
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.Post(entity))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }
        
        [Test]
        public void WithSafeResilience_GetRequestToInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = _returnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity { Id = id });
            api.AllowPartialMapping();
            var returnClientFactory = _basicReturnClientFactory
                .WithSafeResilience(getDelay: _ => 0.Seconds())
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }

        [Test]
        public void WithSafeResilience_PostRequestToInternalServerError_ThrowClientRequestException()
        {
            var entity = new BasicEntity { Id = 1 };
            using var api = _returnApiMockFactory.MockFlakyPostMethod(entity);
            api.AllowPartialMapping();
            var returnClientFactory = _basicReturnClientFactory
                .WithSafeResilience(getDelay: _ => 0.Seconds())
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.Post(entity))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }

        [Test]
        public void WithIdempotentResilience_GetRequestToInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = _returnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity { Id = id });
            api.AllowPartialMapping();
            var returnClientFactory = _basicReturnClientFactory
                .WithIdempotentResilience(getDelay: _ => 0.Seconds())
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }

        [Test]
        public void WithIdempotentResilience_PostRequestToInternalServerError_ThrowClientRequestException()
        {
            var entity = new BasicEntity { Id = 1 };
            using var api = _returnApiMockFactory.MockFlakyPostMethod(entity);
            api.AllowPartialMapping();
            var returnClientFactory = _basicReturnClientFactory
                .WithIdempotentResilience(getDelay: _ => 0.Seconds())
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.Post(entity))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }

        [Test]
        public void WithCustomResilience_GetRequestToInternalServerError_ThrowClientRequestException()
        {
            using var api = _returnApiMockFactory.MockInternalServerError();
            var returnClientFactory = _basicReturnClientFactory
                .WithCustomResilience(selector => selector
                    .ForMethodOf<IReturnClientWithMetadata>(x => (Func<int, BasicEntity>)x.Get)
                    .Use(getDelay: _ => 0.Seconds()))
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.Get(id: 1))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }

        [Test]
        public void WithCustomResilience_GetRequestToFlakyInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = _returnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity());
            var returnClientFactory = _basicReturnClientFactory
                .WithCustomResilience(selector => selector
                    .ForMethodOf<IReturnClientWithMetadata>(x => (Func<int, BasicEntity>)x.Get)
                    .Use(getDelay: _ => 0.Seconds()))
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithCustomResilience_GetAndPostRequestToInternalServerError_ThrowClientRequestException()
        {
            var returnClientFactory = _basicReturnClientFactory
                .WithCustomResilience(selector => selector
                    .ForMethodOf<IReturnClientWithMetadata>(x => (Func<int, BasicEntity>)x.Get)
                    .Use(getDelay: _ => 0.Seconds())
                    .ForMethodOf<IReturnClientWithMetadata>(x => (Action<BasicEntity>)x.Post)
                    .Use(getDelay: _ => 0.Seconds()))
                .Build();

            using var assertionScope = new AssertionScope();
            using (var _ = _returnApiMockFactory.MockInternalServerError())
            {
                var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
                returnClient.Invoking(x => x.Get(id: 1))
                    .Should()
                    .ThrowExactly<ClientRequestException>();
            }
            using (var _ = _returnApiMockFactory.MockInternalServerError())
            {
                var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
                returnClient.Invoking(x => x.Post(new BasicEntity()))
                    .Should()
                    .ThrowExactly<ClientRequestException>();
            }
        }
        
        [Test]
        public void WithCustomResilience_GetAndPostRequestToFlakyInternalServerError_NotThrow()
        {
            const int id = 1;
            var entity = new BasicEntity();
            var returnClientFactory = _basicReturnClientFactory
                .WithCustomResilience(selector => selector
                    .ForMethodOf<IReturnClientWithMetadata>(x => (Func<int, BasicEntity>)x.Get)
                    .Use(getDelay: _ => 0.Seconds())
                    .ForMethodOf<IReturnClientWithMetadata>(x => (Action<BasicEntity>)x.Post)
                    .Use(getDelay: _ => 0.Seconds()))
                .Build();
            
            using var assertionScope = new AssertionScope();
            using (var _ = _returnApiMockFactory.MockFlakyGetMethod(id, entity))
            {
                var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
                returnClient.Invoking(x => x.Get(id))
                    .Should()
                    .NotThrow();
            }
            using (var _ = _returnApiMockFactory.MockFlakyPostMethod(entity))
            {
                var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
                returnClient.Invoking(x => x.Post(entity))
                    .Should()
                    .NotThrow();
            }
        }
        
        [Test]
        public void WithCustomResilience_ForAllMethodsToFlakyInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = _returnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity());
            var returnClientFactory = _basicReturnClientFactory
                .WithCustomResilience(selector => selector
                    .ForAllMethods()
                    .Use(getDelay: _ => 0.Seconds()))
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithCustomResilience_ForAllMethodsExceptGetRequestToInternalServerError_ThrowClientRequestException()
        {
            using var api = _returnApiMockFactory.MockInternalServerError();
            var returnClientFactory = _basicReturnClientFactory
                .WithCustomResilience(selector => selector
                    .ForAllMethods()
                    .Use(getDelay: _ => 0.Seconds())
                    .ForMethodOf<IReturnClientWithMetadata>(x => (Func<int, BasicEntity>)x.Get)
                    .DoNotUse())
                .Build();

            var returnClient = returnClientFactory.Create<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString());
            returnClient.Invoking(x => x.Get(id: 1))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }
    }
}
