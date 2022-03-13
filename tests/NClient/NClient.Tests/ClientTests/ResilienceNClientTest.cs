using System;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using NClient.Exceptions;
using NClient.Providers.Resilience;
using NClient.Providers.Resilience.Polly;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NClient.Tests.ClientTests.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class ResilienceNClientTest : ClientTestBase<IReturnClientWithMetadata>
    {
        [Test]
        public void AsResilientInvoke_InternalServerError_ThrowClientRequestException()
        {
            using var api = ReturnApiMockFactory.MockInternalServerError();
            var resiliencePolicy = new DefaultPollyResiliencePolicyProvider<HttpRequestMessage, HttpResponseMessage>(
                new ResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>(
                    maxRetries: 2,
                    getDelay: _ => 0.Seconds(),
                    shouldRetry: context => !context.Response.IsSuccessStatusCode));
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .Build();

            returnClient.Invoking(x => x.AsResilient().Invoke(client => client.Get(1), resiliencePolicy))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }
        
        [Test]
        public void AsResilientInvoke_FlakyInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = ReturnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity());
            var resiliencePolicy = new DefaultPollyResiliencePolicyProvider<HttpRequestMessage, HttpResponseMessage>(
                new ResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>(
                    maxRetries: 2,
                    getDelay: _ => 0.Seconds(),
                    shouldRetry: context => !context.Response.IsSuccessStatusCode));
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .Build();

            returnClient.Invoking(x => x.AsResilient().Invoke(client => client.Get(id), resiliencePolicy))
                .Should()
                .NotThrow();
        }

        [Test]
        public void WithoutResilience_FlakyInternalServerError_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = ReturnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity());
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithoutResilience()
                .Build();

            returnClient.Invoking(x => x.Get(id))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }
        
        [Test]
        public void WithoutResilience_FlakyInternalServerError_OverrideFullResilienceAndThrowClientRequestException()
        {
            const int id = 1;
            using var api = ReturnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity());
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithFullResilience(getDelay: _ => TimeSpan.FromSeconds(0))
                .WithoutResilience()
                .Build();

            returnClient.Invoking(x => x.Get(id))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }

        [Test]
        public void WithFullResilience_GetRequestToFlakyInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = ReturnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity { Id = id });
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithFullResilience(getDelay: _ => 0.Seconds())
                .Build();

            returnClient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithFullResilience_PostRequestToFlakyInternalServerError_NotThrow()
        {
            var entity = new BasicEntity { Id = 1 };
            using var api = ReturnApiMockFactory.MockFlakyPostMethod(entity);
            api.AllowPartialMapping();
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithFullResilience(getDelay: _ => 0.Seconds())
                .Build();

            returnClient.Invoking(x => x.Post(entity))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithFullResilience_GetIHttpResponseToInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = ReturnApiMockFactory.MockInternalServerError();
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithFullResilience(getDelay: _ => 0.Seconds())
                .Build();

            returnClient.Invoking(x => x.GetIHttpResponse(id))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithFullResilience_GetHttpResponseMessageToInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = ReturnApiMockFactory.MockInternalServerError();
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithFullResilience(getDelay: _ => 0.Seconds())
                .Build();

            returnClient.Invoking(x => x.GetHttpResponseMessage(id))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithFullResilience_ExceptPostRequestToFlakyInternalServerError_ThrowClientRequestException()
        {
            var entity = new BasicEntity { Id = 1 };
            using var api = ReturnApiMockFactory.MockFlakyPostMethod(entity);
            api.AllowPartialMapping();
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithFullResilience(getDelay: _ => 0.Seconds())
                .WithResilience(x => x
                    .ForMethod(client => (Action<BasicEntity>) client.Post)
                    .DoNotUse())
                .Build();

            returnClient.Invoking(x => x.Post(entity))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }
        
        [Test]
        public void WithSafeResilience_GetRequestToInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = ReturnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity { Id = id });
            api.AllowPartialMapping();
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithSafeResilience(getDelay: _ => 0.Seconds())
                .Build();

            returnClient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }

        [Test]
        public void WithSafeResilience_PostRequestToInternalServerError_ThrowClientRequestException()
        {
            var entity = new BasicEntity { Id = 1 };
            using var api = ReturnApiMockFactory.MockFlakyPostMethod(entity);
            api.AllowPartialMapping();
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithSafeResilience(getDelay: _ => 0.Seconds())
                .Build();

            returnClient.Invoking(x => x.Post(entity))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }

        [Test]
        public void WithIdempotentResilience_GetRequestToInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = ReturnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity { Id = id });
            api.AllowPartialMapping();
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithIdempotentResilience(getDelay: _ => 0.Seconds())
                .Build();

            returnClient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }

        [Test]
        public void WithIdempotentResilience_PostRequestToInternalServerError_ThrowClientRequestException()
        {
            var entity = new BasicEntity { Id = 1 };
            using var api = ReturnApiMockFactory.MockFlakyPostMethod(entity);
            api.AllowPartialMapping();
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithIdempotentResilience(getDelay: _ => 0.Seconds())
                .Build();

            returnClient.Invoking(x => x.Post(entity))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }

        [Test]
        public void WithCustomResilience_GetRequestToInternalServerError_ThrowClientRequestException()
        {
            using var api = ReturnApiMockFactory.MockInternalServerError();
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithResilience(selector => selector
                    .ForMethod(x => (Func<int, BasicEntity>) x.Get)
                    .Use(getDelay: _ => 0.Seconds()))
                .Build();

            returnClient.Invoking(x => x.Get(id: 1))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }

        [Test]
        public void WithCustomResilience_GetRequestToFlakyInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = ReturnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity());
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithResilience(selector => selector
                    .ForMethod(x => (Func<int, BasicEntity>) x.Get)
                    .Use(getDelay: _ => 0.Seconds()))
                .Build();

            returnClient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithCustomResilience_GetAndPostRequestToInternalServerError_ThrowClientRequestException()
        {
            using var assertionScope = new AssertionScope();
            using (var api = ReturnApiMockFactory.MockInternalServerError())
            {
                NClientGallery.Clients
                    .GetRest()
                    .For<IReturnClientWithMetadata>(host: api.Urls.First())
                    .WithResilience(selector => selector
                        .ForMethod(x => (Func<int, BasicEntity>) x.Get)
                        .Use(getDelay: _ => 0.Seconds())
                        .ForMethod(x => (Action<BasicEntity>) x.Post)
                        .Use(getDelay: _ => 0.Seconds()))
                    .Build()
                    .Invoking(x => x.Get(id: 1))
                    .Should()
                    .ThrowExactly<ClientRequestException>();
            }
            using (var api = ReturnApiMockFactory.MockInternalServerError())
            {
                NClientGallery.Clients
                    .GetRest()
                    .For<IReturnClientWithMetadata>(host: api.Urls.First())
                    .WithResilience(selector => selector
                        .ForMethod(x => (Func<int, BasicEntity>) x.Get)
                        .Use(getDelay: _ => 0.Seconds())
                        .ForMethod(x => (Action<BasicEntity>) x.Post)
                        .Use(getDelay: _ => 0.Seconds()))
                    .Build()
                    .Invoking(x => x.Post(new BasicEntity()))
                    .Should()
                    .ThrowExactly<ClientRequestException>();
            }
        }
        
        [Test]
        public void WithCustomResilience_GetAndPostRequestToFlakyInternalServerError_NotThrow()
        {
            const int id = 1;
            var entity = new BasicEntity();

            using var assertionScope = new AssertionScope();
            using (var api = ReturnApiMockFactory.MockFlakyGetMethod(id, entity))
            {
                NClientGallery.Clients
                    .GetRest()
                    .For<IReturnClientWithMetadata>(host: api.Urls.First())
                    .WithResilience(selector => selector
                        .ForMethod(x => (Func<int, BasicEntity>) x.Get)
                        .Use(getDelay: _ => 0.Seconds())
                        .ForMethod(x => (Action<BasicEntity>) x.Post)
                        .Use(getDelay: _ => 0.Seconds()))
                    .Build()
                    .Invoking(x => x.Get(id))
                    .Should()
                    .NotThrow();
            }
            using (var api = ReturnApiMockFactory.MockFlakyPostMethod(entity))
            {
                NClientGallery.Clients
                    .GetRest()
                    .For<IReturnClientWithMetadata>(host: api.Urls.First())
                    .WithResilience(selector => selector
                        .ForMethod(x => (Func<int, BasicEntity>) x.Get)
                        .Use(getDelay: _ => 0.Seconds())
                        .ForMethod(x => (Action<BasicEntity>) x.Post)
                        .Use(getDelay: _ => 0.Seconds()))
                    .Build()
                    .Invoking(x => x.Post(entity))
                    .Should()
                    .NotThrow();
            }
        }
        
        [Test]
        public void WithCustomResilience_ForAllMethodsToFlakyInternalServerError_NotThrow()
        {
            const int id = 1;
            using var api = ReturnApiMockFactory.MockFlakyGetMethod(id, new BasicEntity());
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithResilience(selector => selector
                    .ForAllMethods()
                    .Use(getDelay: _ => 0.Seconds()))
                .Build();

            returnClient.Invoking(x => x.Get(id))
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void WithCustomResilience_ForAllMethodsExceptGetRequestToInternalServerError_ThrowClientRequestException()
        {
            using var api = ReturnApiMockFactory.MockInternalServerError();
            var returnClient = NClientGallery.Clients
                .GetRest()
                .For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithResilience(selector => selector
                    .ForAllMethods()
                    .Use(getDelay: _ => 0.Seconds())
                    .ForMethod(x => (Func<int, BasicEntity>) x.Get)
                    .DoNotUse())
                .Build();

            returnClient.Invoking(x => x.Get(id: 1))
                .Should()
                .ThrowExactly<ClientRequestException>();
        }
    }
}
