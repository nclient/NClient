﻿using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Api.Tests.BasicClientUseCases
{
    [Parallelizable]
    public class SerializerTest
    {
        private INClientOptionalBuilder<IBasicClientWithMetadata, HttpRequestMessage, HttpResponseMessage> _optionalBuilder = null!;
        private BasicApiMockFactory _api = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _api = new BasicApiMockFactory(PortsPool.Get());
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            PortsPool.Put(_api.ApiUri.Port);
        }
        
        [SetUp]
        public void SetUp()
        {
            _optionalBuilder = NClientGallery.Clients.GetBasic().For<IBasicClientWithMetadata>(_api.ApiUri.ToString());
        }
        
        [Test]
        public async Task NClientBuilder_WithNewtonsoft_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .WithNewtonsoftJsonSerialization()
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}
