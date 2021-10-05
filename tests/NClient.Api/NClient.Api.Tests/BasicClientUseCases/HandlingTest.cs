﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Abstractions.Builders;
using NClient.Api.Tests.Helpers;
using NClient.Testing.Common.Apis;
using NClient.Tests.Clients;
using NUnit.Framework;

namespace NClient.Api.Tests.BasicClientUseCases
{
    [Parallelizable]
    public class HandlingTest
    {
        private INClientOptionalBuilder<IBasicClientWithMetadata, HttpRequestMessage, HttpResponseMessage> _optionalBuilder = null!;
        private BasicApiMockFactory _api = null!;

        [SetUp]
        public void SetUp()
        {
            _api = new BasicApiMockFactory(5024);
            _optionalBuilder = NClientGallery.NativeClients.GetBasic().For<IBasicClientWithMetadata>(_api.ApiUri.ToString());
        }
        
        [Test]
        public async Task NClientBuilder_WithSingleCustomHandler_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .WithCustomHandling(new CustomHandler())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_WithCollectionOfCustomHandlers_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .WithCustomHandling(new CustomHandler(), new CustomHandler())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test, Ignore("Not Implemented")]
        public Task NClientBuilder_WithAdditionalSingleCustomHandler_NotThrow()
        {
            throw new NotImplementedException();
        }
        
        [Test, Ignore("Not Implemented")]
        public Task NClientBuilder_WithAdditionalCollectionOfCustomHandlers_NotThrow()
        {
            throw new NotImplementedException();
        }
    }
}