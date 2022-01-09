using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using NClient.Providers.Results.HttpResults;
using NUnit.Framework;

namespace NClient.Providers.Mapping.HttpResponses.Tests
{
    [Parallelizable]
    public class HttpResponseDeconstructTest
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
        public void IHttpResponseWithDataAndError_Deconstruct()
        {
            var resultData = _fixture.Create<Int32>();
            var error = _fixture.Create<string>();
            var response = new HttpResponseWithError<Int32, string>(_fixture.Create<HttpResponse>(), resultData, error);
            
            var (data, err, _) = response;
            data.Should().Be(resultData);
            err.Should().Be(error);
        }

        [Test]
        public void IHttpResponseWithData_Deconstruct()
        {
            var resultData = _fixture.Create<Int32>();
            var response = new HttpResponse<Int32>(_fixture.Build<HttpResponse>().Create(), resultData);

            var (data, _) = response;
            data.Should().Be(resultData);
        }
    }
}
