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
            var resultData = _fixture.Create<Int32>();
            var error = _fixture.Create<string>();
            var response = new ResponseWithError<Int32, string>(_fixture.Build<Response>().Create(), _fixture.Build<Request>().Create(), resultData, error);

            var (data, err) = response;
            data.Should().Be(resultData);
            err.Should().Be(error);
        }

        [Test]
        public void IResponseWithData_Deconstruct()
        {
            var resultData = _fixture.Create<Int32>();
            var response = new Response<Int32>(_fixture.Build<Response>().Create(), _fixture.Build<Request>().Create(), resultData);

            var (responseData, _) = response;
            responseData.Should().Be(resultData);
        }
    }
}
