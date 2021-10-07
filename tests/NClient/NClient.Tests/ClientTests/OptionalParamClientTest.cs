using System.Threading.Tasks;
using FluentAssertions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class OptionalParamClientTest
    {
        private IOptionalParamWithMetadata _optionalParamClient = null!;
        private OptionalParamApiMockFactory _optionalParamApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _optionalParamApiMockFactory = new OptionalParamApiMockFactory(port: 5018);

            _optionalParamClient = NClientGallery.NativeClients
                .GetBasic()
                .For<IOptionalParamWithMetadata>(_optionalParamApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task OptionalParamClient_GetAsync_IntInBody()
        {
            const int id = 2;
            using var api = _optionalParamApiMockFactory.MockGetMethod(id);

            var result = await _optionalParamClient.GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task OptionalParamClient_GetAsyncWithDefaultValue_IntInBody()
        {
            const int id = 1;
            using var api = _optionalParamApiMockFactory.MockGetMethod(id);

            var result = await _optionalParamClient.GetAsync();

            result.Should().Be(id);
        }

        [Test]
        public async Task OptionalParamClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _optionalParamApiMockFactory.MockPostMethod(entity);

            await _optionalParamClient
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task OptionalParamClient_PostAsyncWithDefaultValue_NotThrow()
        {
            using var api = _optionalParamApiMockFactory.MockPostMethod();

            await _optionalParamClient
                .Invoking(async x => await x.PostAsync())
                .Should()
                .NotThrowAsync();
        }
    }
}
