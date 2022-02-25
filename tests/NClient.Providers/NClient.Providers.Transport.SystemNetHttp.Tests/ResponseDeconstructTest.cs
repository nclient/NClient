using System;
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
        public void IResponseWithDataAndError_Deconstruct()
        {
            var expectedData = _fixture.Create<Int32>();
            var expectedError = _fixture.Create<string>();
            var actualResponse = new ResponseWithError<Int32, string>(_fixture.Build<Response>().Create(), _fixture.Build<Request>().Create(), expectedData, expectedError);

            var (data, error, response) = actualResponse;
            data.Should().Be(expectedData);
            error.Should().Be(expectedError);
            response.Should().Be(actualResponse);
        }

        [Test]
        public void IResponseWithData_Deconstruct()
        {
            var expectedData = _fixture.Create<Int32>();
            var actualResponse = new Response<Int32>(_fixture.Build<Response>().Create(), _fixture.Build<Request>().Create(), expectedData);

            var (data, response) = actualResponse;
            data.Should().Be(expectedData);
            response.Should().Be(actualResponse);
        }
    }
}
