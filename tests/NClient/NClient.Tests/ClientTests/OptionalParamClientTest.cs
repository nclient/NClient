using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class OptionalParamClientTest
    {
        [Test]
        public async Task OptionalParamClient_GetAsync_IntInBody()
        {
            const int id = 2;
            using var api = OptionalParamApiMockFactory.MockGetMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IOptionalParamWithMetadata>(host: api.Urls.First()).Build()
                .GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task OptionalParamClient_GetAsyncWithDefaultValue_IntInBody()
        {
            const int id = 1;
            using var api = OptionalParamApiMockFactory.MockGetMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IOptionalParamWithMetadata>(host: api.Urls.First()).Build()
                .GetAsync();

            result.Should().Be(id);
        }

        [Test]
        public async Task OptionalParamClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = OptionalParamApiMockFactory.MockPostMethod(entity);

            await NClientGallery.Clients.GetRest().For<IOptionalParamWithMetadata>(host: api.Urls.First()).Build()
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task OptionalParamClient_PostAsyncWithDefaultValue_NotThrow()
        {
            using var api = OptionalParamApiMockFactory.MockPostMethod();

            await NClientGallery.Clients.GetRest().For<IOptionalParamWithMetadata>(host: api.Urls.First()).Build()
                .Invoking(async x => await x.PostAsync())
                .Should()
                .NotThrowAsync();
        }
    }
}
