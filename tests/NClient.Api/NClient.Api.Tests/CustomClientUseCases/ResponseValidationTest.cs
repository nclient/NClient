﻿using System.Threading.Tasks;
using FluentAssertions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NUnit.Framework;

namespace NClient.Api.Tests.CustomClientUseCases
{
    [Parallelizable]
    public class ResponseValidationTest
    {
        private INClientTransportBuilder<IBasicClientWithMetadata> _optionalBuilder = null!;
        private BasicApiMockFactory _api = null!;

        [SetUp]
        public void SetUp()
        {
            _api = new BasicApiMockFactory(5028);
            _optionalBuilder = new CustomNClientBuilder().For<IBasicClientWithMetadata>(_api.ApiUri.ToString());
        }
        
        [Test]
        public async Task CustomNClientBuilder_WithRestSharpResponseValidation_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .UsingRestSharpHttpClient()
                .UsingJsonSerializer()
                .WithRestSharpResponseValidation()
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}