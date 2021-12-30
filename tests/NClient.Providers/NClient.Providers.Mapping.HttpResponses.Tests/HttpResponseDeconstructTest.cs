using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using NClient.Providers.Results.HttpResults;
using NClient.Providers.Transport;
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
            _fixture.Customizations.Add(
                new StringGenerator(() =>
                    Guid.NewGuid().ToString().Substring(0, 10)));
            _fixture.Customizations.Add(
                new CharSequenceGenerator(() => Guid.NewGuid().ToString().ToCharArray()));
        }

        [Test]
        public void IResponseWithDataAndError_Deconstruct()
        {
            var resultData = _fixture.Create<Int32>();
            var error = _fixture.Build<string>().Create();
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

        [Test]
        public void IHttpResponseWithDataAndError_Deconstruct()
        {
            var resultData = _fixture.Create<Int32>();
            var error = _fixture.Build<string>().Create();
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
