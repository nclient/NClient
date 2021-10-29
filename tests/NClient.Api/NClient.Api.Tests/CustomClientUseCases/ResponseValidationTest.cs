using System.Threading.Tasks;
using FluentAssertions;
using NClient.Providers.Api.Rest.Extensions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Api.Tests.CustomClientUseCases
{
    [Parallelizable]
    public class ResponseValidationTest
    {
        private INClientApiBuilder<IBasicClientWithMetadata> _optionalBuilder = null!;
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
            _optionalBuilder = new CustomNClientBuilder().For<IBasicClientWithMetadata>(_api.ApiUri.ToString());
        }
        
        [Test]
        public async Task CustomNClientBuilder_WithRestSharpResponseValidation_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .UsingRestApi()
                .UsingRestSharpTransport()
                .UsingJsonSerializer()
                .WithRestSharpResponseValidation()
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}
