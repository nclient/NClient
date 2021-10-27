using System.Net.Http;
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
        private INClientOptionalBuilder<IRestClientWithMetadata, HttpRequestMessage, HttpResponseMessage> _preConfiguredBasicClient = null!;
        private RestApiMockFactory _restApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _restApiMockFactory = new RestApiMockFactory(port: 5030);

            _preConfiguredBasicClient = new CustomNClientBuilder()
                .For<IRestClientWithMetadata>(_restApiMockFactory.ApiUri.ToString())
                .UsingSystemHttpTransport()
                .UsingRestApi()
                .UsingSystemJsonSerializer();
        }
        
        [Test]
        public async Task NotResponseValidation_GetIntAsync_Id()
        {
            const int id = 1;
            using var api = _restApiMockFactory.MockIntGetMethod(id);

            var result = await _preConfiguredBasicClient
                .WithoutResponseValidation()
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task NotResponseValidation_GetStringAsync_Id()
        {
            const string id = "1";
            using var api = _restApiMockFactory.MockStringGetMethod(id);

            var result = await _preConfiguredBasicClient
                .WithoutResponseValidation()
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task ResponseValidation_GetIntAsync_Id()
        {
            const int id = 1;
            using var api = _restApiMockFactory.MockIntGetMethod(id);

            var result = await _preConfiguredBasicClient
                .WithSystemResponseValidation()
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task ResponseValidation_GetStringAsync_Id()
        {
            const string id = "1";
            using var api = _restApiMockFactory.MockStringGetMethod(id);

            var result = await _preConfiguredBasicClient
                .WithSystemResponseValidation()
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task NotResponseValidation_GetNotFoundInt_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = _restApiMockFactory.MockNotFoundIntGetMethod(id);

            await _preConfiguredBasicClient.Invoking(async x => await x.WithoutResponseValidation()
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
        
        [Test, Ignore("it doesn't work with a string")]
        public async Task NotResponseValidation_GetNotFoundString_ThrowClientRequestException()
        {
            const string id = "1";
            using var api = _restApiMockFactory.MockNotFoundStringGetMethod(id);

            await _preConfiguredBasicClient.Invoking(async x => await x.WithoutResponseValidation()
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
        
        [Test]
        public async Task SystemResponseValidation_GetIntAsync_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = _restApiMockFactory.MockNotFoundIntGetMethod(id);

            await _preConfiguredBasicClient.Invoking(async x => await x
                    .WithSystemResponseValidation()
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
        
        [Test]
        public async Task SystemResponseValidation_GetStringAsync_ThrowClientRequestException()
        {
            const string id = "1";
            using var api = _restApiMockFactory.MockNotFoundStringGetMethod(id);

            await _preConfiguredBasicClient.Invoking(async x => await x
                    .WithSystemResponseValidation()
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
    }
}
