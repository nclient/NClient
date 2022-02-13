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
        public void IResponseWithDataAndError_Deconstruct()
        {
            var resultData = _fixture.Create<int>();
            var error = _fixture.Create<string>();
            var response = new ResponseWithError<int, string>(
                _fixture.Build<Response>().With(x => x.Content, CreateFakeContent(resultData)).Create(), 
                _fixture.Build<Request>().Create(), 
                resultData, error);

            var (data, err) = response;
            data.Should().Be(resultData);
            err.Should().Be(error);
        }

        [Test]
        public void IResponseWithData_Deconstruct()
        {
            var resultData = _fixture.Create<int>();
            var response = new Response<int>(
                _fixture.Build<Response>().With(x => x.Content, CreateFakeContent(resultData)).Create(), 
                _fixture.Build<Request>().Create(), 
                resultData);

            var (responseData, _) = response;
            responseData.Should().Be(resultData);
        }
        private static Content CreateFakeContent(int resultData)
        {
            return new Content(new MemoryStream(Encoding.UTF8.GetBytes(resultData.ToString())), Encoding.UTF8.WebName);
        }
    }
}
