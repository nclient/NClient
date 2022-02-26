using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Exceptions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Helpers;
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

            var result = await new NClientBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First().ToUri())
                .UsingRestApi()
                .UsingSystemNetHttpTransport()
                .UsingSystemTextJsonSerialization()
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

            var result = await new NClientBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First().ToUri())
                .UsingRestApi()
                .UsingSystemNetHttpTransport()
                .UsingSystemTextJsonSerialization()
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

            var result = await new NClientBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First().ToUri())
                .UsingRestApi()
                .UsingSystemNetHttpTransport()
                .UsingSystemTextJsonSerialization()
                .WithAdvancedResponseValidation(x => x
                    .ForTransport().UseSystemNetHttpResponseValidation())
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task ResponseValidation_GetStringAsync_Id()
        {
            const string id = "1";
            using var api = RestApiMockFactory.MockStringGetMethod(id);

            var result = await new NClientBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First().ToUri())
                .UsingRestApi()
                .UsingSystemNetHttpTransport()
                .UsingSystemTextJsonSerialization()
                .WithAdvancedResponseValidation(x => x
                    .ForTransport().UseSystemNetHttpResponseValidation())
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task NotResponseValidation_GetNotFoundInt_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = RestApiMockFactory.MockNotFoundIntGetMethod(id);

            await new NClientBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First().ToUri())
                .UsingRestApi()
                .UsingSystemNetHttpTransport()
                .UsingSystemTextJsonSerialization()
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

            await new NClientBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First().ToUri())
                .UsingRestApi()
                .UsingSystemNetHttpTransport()
                .UsingSystemTextJsonSerialization()
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

            await new NClientBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First().ToUri())
                .UsingRestApi()
                .UsingSystemNetHttpTransport()
                .UsingSystemTextJsonSerialization()
                .Invoking(async x => await x
                    .WithAdvancedResponseValidation(setter => setter
                        .ForTransport().UseSystemNetHttpResponseValidation())
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
        
        [Test]
        public async Task SystemResponseValidation_GetStringAsync_ThrowClientRequestException()
        {
            const string id = "1";
            using var api = RestApiMockFactory.MockNotFoundStringGetMethod(id);

            await new NClientBuilder()
                .For<IRestClientWithMetadata>(api.Urls.First().ToUri())
                .UsingRestApi()
                .UsingSystemNetHttpTransport()
                .UsingSystemTextJsonSerialization()
                .Invoking(async x => await x
                    .WithAdvancedResponseValidation(setter => setter
                        .ForTransport().UseSystemNetHttpResponseValidation())
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
    }
}
