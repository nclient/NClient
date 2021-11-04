using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Exceptions;
using NClient.Providers.Api.Rest.Extensions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NUnit.Framework;

namespace NClient.Standalone.Tests
{
    [Parallelizable]
    public class ResponseValidationTest
    {
        [Test]
        public async Task NotResponseValidation_GetIntAsync_Id()
        {
            const int id = 1;
            using var api = RestApiMockFactory.MockIntGetMethod(id);

            var result = await new NClientAdvancedBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemHttpTransport()
                .UsingSystemJsonSerialization()
                .WithoutResponseValidation()
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task NotResponseValidation_GetStringAsync_Id()
        {
            const string id = "1";
            using var api = RestApiMockFactory.MockStringGetMethod(id);

            var result = await new NClientAdvancedBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemHttpTransport()
                .UsingSystemJsonSerialization()
                .WithoutResponseValidation()
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task ResponseValidation_GetIntAsync_Id()
        {
            const int id = 1;
            using var api = RestApiMockFactory.MockIntGetMethod(id);

            var result = await new NClientAdvancedBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemHttpTransport()
                .UsingSystemJsonSerialization()
                .WithResponseValidation(x => x
                    .ForTransport().UseSystemResponseValidation())
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task ResponseValidation_GetStringAsync_Id()
        {
            const string id = "1";
            using var api = RestApiMockFactory.MockStringGetMethod(id);

            var result = await new NClientAdvancedBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemHttpTransport()
                .UsingSystemJsonSerialization()
                .WithResponseValidation(x => x
                    .ForTransport().UseSystemResponseValidation())
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task NotResponseValidation_GetNotFoundInt_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = RestApiMockFactory.MockNotFoundIntGetMethod(id);

            await new NClientAdvancedBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemHttpTransport()
                .UsingSystemJsonSerialization()
                .Invoking(async x => await x.WithoutResponseValidation()
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
        
        [Test, Ignore("it doesn't work with a string")]
        public async Task NotResponseValidation_GetNotFoundString_ThrowClientRequestException()
        {
            const string id = "1";
            using var api = RestApiMockFactory.MockNotFoundStringGetMethod(id);

            await new NClientAdvancedBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemHttpTransport()
                .UsingSystemJsonSerialization()
                .Invoking(async x => await x.WithoutResponseValidation()
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
        
        [Test]
        public async Task SystemResponseValidation_GetIntAsync_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = RestApiMockFactory.MockNotFoundIntGetMethod(id);

            await new NClientAdvancedBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemHttpTransport()
                .UsingSystemJsonSerialization()
                .Invoking(async x => await x
                    .WithResponseValidation(setter => setter
                        .ForTransport().UseSystemResponseValidation())
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
        
        [Test]
        public async Task SystemResponseValidation_GetStringAsync_ThrowClientRequestException()
        {
            const string id = "1";
            using var api = RestApiMockFactory.MockNotFoundStringGetMethod(id);

            await new NClientAdvancedBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingSystemHttpTransport()
                .UsingSystemJsonSerialization()
                .Invoking(async x => await x
                    .WithResponseValidation(setter => setter
                        .ForTransport().UseSystemResponseValidation())
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
    }
}
