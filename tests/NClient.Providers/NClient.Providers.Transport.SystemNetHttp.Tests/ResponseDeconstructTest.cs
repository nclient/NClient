using System.IO;
using System.Text;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using NUnit.Framework;

namespace NClient.Providers.Transport.SystemNetHttp.Tests
{
    [Parallelizable]
    public class ResponseDeconstructTest
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Fixture _fixture = new();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customize(new AutoMoqCustomization());
        }

        [Test]
        public void IResponseWithDataOrError_Deconstruct()
        {
            var expectedData = _fixture.Create<int>();
            var expectedData = _fixture.Create<string>();
            var actualResponse = new ResponseWithError<int, string>(
                _fixture.Build<Response>().With(x => x.Content, CreateFakeContent(expectedData)).Create(), 
                _fixture.Build<Request>().Create(), 
                expectedData, expectedData);

            var (data, error, response) = actualResponse;
            data.Should().Be(expectedData);
            error.Should().Be(expectedError);
            response.Should().Be(actualResponse);
        }

        [Test]
        public void IResponseWithData_Deconstruct()
        {
            var expectedData = _fixture.Create<int>();
            var actualResponse = new Response<int>(
                _fixture.Build<Response>().With(x => x.Content, CreateFakeContent(expectedData)).Create(), 
                _fixture.Build<Request>().Create(), 
                expectedData);

            var (data, response) = actualResponse;
            data.Should().Be(expectedData);
            response.Should().Be(actualResponse);
        }
        private static Content CreateFakeContent(int resultData)
        {
            return new Content(new MemoryStream(Encoding.UTF8.GetBytes(resultData.ToString())), Encoding.UTF8.WebName);
        }
    }
}
