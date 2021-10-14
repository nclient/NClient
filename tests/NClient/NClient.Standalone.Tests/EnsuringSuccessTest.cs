using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Abstractions.Building;
using NClient.Exceptions;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Serialization.System;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NUnit.Framework;

namespace NClient.Standalone.Tests
{
    [Parallelizable]
    public class EnsuringSuccessTest
    {
        private INClientOptionalBuilder<IRestClientWithMetadata, HttpRequestMessage, HttpResponseMessage> _preConfiguredBasicClient = null!;
        private RestApiMockFactory _restApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _restApiMockFactory = new RestApiMockFactory(port: 5030);

            _preConfiguredBasicClient = new CustomNClientBuilder()
                .For<IRestClientWithMetadata>(_restApiMockFactory.ApiUri.ToString())
                .UsingSystemHttpClient()
                .UsingSystemJsonSerializer();
        }
        
        [Test]
        public async Task NotEnsuringSuccess_GetIntAsync_Id()
        {
            const int id = 1;
            using var api = _restApiMockFactory.MockIntGetMethod(id);

            var result = await _preConfiguredBasicClient
                .NotEnsuringSuccess()
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task NotEnsuringSuccess_GetStringAsync_Id()
        {
            const string id = "1";
            using var api = _restApiMockFactory.MockStringGetMethod(id);

            var result = await _preConfiguredBasicClient
                .NotEnsuringSuccess()
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task EnsuringSuccess_GetIntAsync_Id()
        {
            const int id = 1;
            using var api = _restApiMockFactory.MockIntGetMethod(id);

            var result = await _preConfiguredBasicClient
                .EnsuringSystemSuccess()
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task EnsuringSuccess_GetStringAsync_Id()
        {
            const string id = "1";
            using var api = _restApiMockFactory.MockStringGetMethod(id);

            var result = await _preConfiguredBasicClient
                .EnsuringSystemSuccess()
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task NotEnsuringSuccess_GetNotFoundInt_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = _restApiMockFactory.MockNotFoundIntGetMethod(id);

            await _preConfiguredBasicClient.Invoking(async x => await x.NotEnsuringSuccess()
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
        
        [Test, Ignore("it doesn't work with a string")]
        public async Task NotEnsuringSuccess_GetNotFoundString_ThrowClientRequestException()
        {
            const string id = "1";
            using var api = _restApiMockFactory.MockNotFoundStringGetMethod(id);

            await _preConfiguredBasicClient.Invoking(async x => await x.NotEnsuringSuccess()
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
        
        [Test]
        public async Task EnsuringSystemSuccess_GetIntAsync_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = _restApiMockFactory.MockNotFoundIntGetMethod(id);

            await _preConfiguredBasicClient.Invoking(async x => await x
                    .EnsuringSystemSuccess()
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
        
        [Test]
        public async Task EnsuringSystemSuccess_GetStringAsync_ThrowClientRequestException()
        {
            const string id = "1";
            using var api = _restApiMockFactory.MockNotFoundStringGetMethod(id);

            await _preConfiguredBasicClient.Invoking(async x => await x
                    .EnsuringSystemSuccess()
                    .Build()
                    .GetAsync(id))
                .Should().ThrowExactlyAsync<ClientRequestException>();
        }
    }
}
