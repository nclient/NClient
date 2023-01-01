using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NClient.Annotations.Http;
using NClient.Common.Helpers;
using NClient.Models;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Providers.Api.Rest.Tests.RequestBuilderTests
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    public class RequestBuilderBodyTest : RequestBuilderTestBase
    {
        private interface ICustomTypeBody { [GetMethod] int Get([BodyParam] BasicEntity entity); }

        [Test]
        public async Task Build_CustomTypeBody_JsonObjectInBody()
        {
            var basicEntity = new BasicEntity { Id = 1 };
            var expectedContent = Serializer.Serialize(basicEntity);

            var request = BuildRequest(BuildMethod<ICustomTypeBody>(), basicEntity);

            request.Id.Should().Be(RequestId);
            request.Type.Should().Be(RequestType.Read);
            request.Resource.Should().Be(RequestUri);
            request.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            { 
                AcceptMetadata
            }, opts => opts.WithoutStrictOrdering());
            
            request.Content.Should().NotBeNull();
            request.Content!.Encoding.Should().Be(RequestEncoding);
            (await request.Content.Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent);
            request.Content.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                ContentEncodingMetadata,
                ContentTypeMetadata,
                GetContentLengthMetadata(expectedContent)
            }, opts => opts.WithoutStrictOrdering());
        }

        private interface IMultipleBodyParameters { [GetMethod] int Get([BodyParam] BasicEntity entity1, [BodyParam] BasicEntity entity2); }

        [Test]
        public async Task Build_MultipleBodyParameters_NotThrow()
        {
            var basicEntity1 = new BasicEntity { Id = 1 };
            var basicEntity2 = new BasicEntity { Id = 2 };
            var expectedContent1 = Serializer.Serialize(basicEntity1);
            var expectedContent2 = Serializer.Serialize(basicEntity2);
            
            var request = BuildRequest(
                BuildMethod<IMultipleBodyParameters>(), basicEntity1, basicEntity2);
            
            request.Id.Should().Be(RequestId);
            request.Type.Should().Be(RequestType.Read);
            request.Resource.Should().Be(RequestUri);
            request.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            { 
                AcceptMetadata
            }, opts => opts.WithoutStrictOrdering());
            
            request.Content.Should().NotBeNull();
            request.Content.Should().BeAssignableTo<MultipartContent>();
            var contents = ((IEnumerable<IContent>) request.Content!).ToArray();
            contents[0].Encoding.Should().Be(RequestEncoding);
            (await contents[0].Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent1);
            contents[0].Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                ContentEncodingMetadata,
                ContentTypeMetadata,
                GetContentLengthMetadata(expectedContent1)
            }, opts => opts.WithoutStrictOrdering());
            contents[1].Encoding.Should().Be(RequestEncoding);
            (await contents[1].Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent2);
            contents[1].Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                ContentEncodingMetadata,
                ContentTypeMetadata,
                GetContentLengthMetadata(expectedContent2)
            }, opts => opts.WithoutStrictOrdering());
        }

        private interface ICustomTypeBodyWithoutAttribute { [GetMethod] int Get(BasicEntity entity); }

        [Test]
        public async Task Build_CustomTypeBodyWithoutAttribute_JsonObjectInBody()
        {
            var basicEntity = new BasicEntity { Id = 1 };
            var expectedContent = Serializer.Serialize(basicEntity);

            var request = BuildRequest(BuildMethod<ICustomTypeBodyWithoutAttribute>(), basicEntity);

            request.Id.Should().Be(RequestId);
            request.Type.Should().Be(RequestType.Read);
            request.Resource.Should().Be(RequestUri);
            request.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            { 
                AcceptMetadata
            }, opts => opts.WithoutStrictOrdering());
            
            request.Content.Should().NotBeNull();
            request.Content!.Encoding.Should().Be(RequestEncoding);
            (await request.Content.Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent);
            request.Content.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                ContentEncodingMetadata,
                ContentTypeMetadata,
                GetContentLengthMetadata(expectedContent)
            }, opts => opts.WithoutStrictOrdering());
        }
        
        private interface IStreamContentBodyParameter { [PostMethod] void Post([BodyParam] IStreamContent streamContent); }

        [Test]
        public async Task Build_StreamContentBodyParameter_NotThrow()
        {
            const string contentType = "application/octet-stream";
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var expectedContent = Serializer.Serialize(entity);
            var streamContent = new StreamContent(
                new MemoryStream(RequestEncoding.GetBytes(expectedContent)), 
                RequestEncoding, 
                contentType);

            var request = BuildRequest(BuildMethod<IStreamContentBodyParameter>(), streamContent);

            request.Id.Should().Be(RequestId);
            request.Type.Should().Be(RequestType.Create);
            request.Resource.Should().Be(RequestUri);
            request.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            { 
                AcceptMetadata
            }, opts => opts.WithoutStrictOrdering());
            
            request.Content.Should().NotBeNull();
            request.Content!.Encoding.Should().Be(RequestEncoding);
            (await request.Content.Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent);
            request.Content.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                ContentEncodingMetadata,
                GetContentTypeMetadata(contentType),
                GetContentDispositionMetadata("attachment; name=\"\"")
            }, opts => opts.WithoutStrictOrdering());
        }
        
        private interface IFormFileBodyParameter { [PostMethod] void Post([BodyParam] IFormFile formFile); }

        [Test]
        public async Task Build_FormFileBodyParameter_NotThrow()
        {
            const string contentType = "application/octet-stream";
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var expectedContent = Serializer.Serialize(entity);
            var formFileContent = new HttpFileContent(
                new MemoryStream(RequestEncoding.GetBytes(expectedContent)), 
                name: "test", 
                fileName: "test.txt", 
                contentType: contentType, 
                contentDisposition: "attachment");

            var request = BuildRequest(BuildMethod<IFormFileBodyParameter>(), formFileContent);

            request.Id.Should().Be(RequestId);
            request.Type.Should().Be(RequestType.Create);
            request.Resource.Should().Be(RequestUri);
            request.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            { 
                AcceptMetadata
            }, opts => opts.WithoutStrictOrdering());
            
            request.Content.Should().NotBeNull();
            (await request.Content!.Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent);
            request.Content.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                GetContentTypeMetadata(contentType),
                GetContentDispositionMetadata("attachment; name=\"test\"; filename=\"test.txt\"")
            }, opts => opts.WithoutStrictOrdering());
        }

        private interface IMultipleBodyParametersWithoutAttributes { [GetMethod] int Get(BasicEntity entity1, BasicEntity entity2); }

        [Test]
        public async Task Build_MultipleBodyParametersWithoutAttributes_NotThrow()
        {
            var basicEntity1 = new BasicEntity { Id = 1 };
            var basicEntity2 = new BasicEntity { Id = 2 };
            var expectedContent1 = Serializer.Serialize(basicEntity1);
            var expectedContent2 = Serializer.Serialize(basicEntity2);
            
            var request = BuildRequest(
                BuildMethod<IMultipleBodyParametersWithoutAttributes>(), basicEntity1, basicEntity2);
            
            request.Id.Should().Be(RequestId);
            request.Type.Should().Be(RequestType.Read);
            request.Resource.Should().Be(RequestUri);
            request.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            { 
                AcceptMetadata
            }, opts => opts.WithoutStrictOrdering());
            
            request.Content.Should().NotBeNull();
            request.Content.Should().BeAssignableTo<MultipartContent>();
            var contents = ((IEnumerable<IContent>) request.Content!).ToArray();
            contents[0].Encoding.Should().Be(RequestEncoding);
            (await contents[0].Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent1);
            contents[0].Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                ContentEncodingMetadata,
                ContentTypeMetadata,
                GetContentLengthMetadata(expectedContent1)
            }, opts => opts.WithoutStrictOrdering());
            contents[1].Encoding.Should().Be(RequestEncoding);
            (await contents[1].Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent2);
            contents[1].Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                ContentEncodingMetadata,
                ContentTypeMetadata,
                GetContentLengthMetadata(expectedContent2)
            }, opts => opts.WithoutStrictOrdering());
        }

        private interface IPrimitiveBody { [GetMethod] int Get([BodyParam] int id); }

        [Test]
        public async Task Build_PrimitiveBody_PrimitiveInBody()
        {
            const int id = 1;
            var expectedContent = Serializer.Serialize(id);

            var request = BuildRequest(BuildMethod<IPrimitiveBody>(), id);

            request.Id.Should().Be(RequestId);
            request.Type.Should().Be(RequestType.Read);
            request.Resource.Should().Be(RequestUri);
            request.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            { 
                AcceptMetadata
            }, opts => opts.WithoutStrictOrdering());
            
            request.Content.Should().NotBeNull();
            request.Content!.Encoding.Should().Be(RequestEncoding);
            (await request.Content.Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent);
            request.Content.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                ContentEncodingMetadata,
                ContentTypeMetadata,
                GetContentLengthMetadata(expectedContent)
            }, opts => opts.WithoutStrictOrdering());
        }

        private interface IMultiplyPrimitiveBodyParameters { [GetMethod] int Get([BodyParam] int id, [BodyParam] string value); }

        [Test]
        public async Task Build_MultiplyPrimitiveBodyParameters_NotThrow()
        {
            const int id = 1;
            const string value = "val";
            var expectedContent1 = Serializer.Serialize(id);
            var expectedContent2 = Serializer.Serialize(value);
            
            var request = BuildRequest(
                BuildMethod<IMultiplyPrimitiveBodyParameters>(), id, value);

            request.Id.Should().Be(RequestId);
            request.Type.Should().Be(RequestType.Read);
            request.Resource.Should().Be(RequestUri);
            request.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            { 
                AcceptMetadata
            }, opts => opts.WithoutStrictOrdering());
            
            request.Content.Should().NotBeNull();
            request.Content.Should().BeAssignableTo<MultipartContent>();
            var contents = ((IEnumerable<IContent>) request.Content!).ToArray();
            contents[0].Encoding.Should().Be(RequestEncoding);
            (await contents[0].Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent1);
            contents[0].Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                ContentEncodingMetadata,
                ContentTypeMetadata,
                GetContentLengthMetadata(expectedContent1)
            }, opts => opts.WithoutStrictOrdering());
            contents[1].Encoding.Should().Be(RequestEncoding);
            (await contents[1].Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent2);
            contents[1].Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                ContentEncodingMetadata,
                ContentTypeMetadata,
                GetContentLengthMetadata(expectedContent2)
            }, opts => opts.WithoutStrictOrdering());
        }

        private interface IPrimitiveWithFormParameter { [PostMethod] void Post([FormParam] int id); }

        [Test]
        public async Task Build_PrimitiveWithFormParameter_NotThrow()
        {
            const int id = 1;
            const string expectedContent = "id=1";

            var request = BuildRequest(BuildMethod<IPrimitiveWithFormParameter>(), id);

            request.Id.Should().Be(RequestId);
            request.Type.Should().Be(RequestType.Create);
            request.Resource.Should().Be(RequestUri);
            request.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            { 
                AcceptMetadata
            }, opts => opts.WithoutStrictOrdering());
            
            request.Content.Should().NotBeNull();
            request.Content!.Encoding.Should().Be(RequestEncoding);
            (await request.Content.Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent);
            request.Content.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                ContentEncodingMetadata,
                GetContentTypeMetadata("application/x-www-form-urlencoded"),
                GetContentLengthMetadata(expectedContent)
            }, opts => opts.WithoutStrictOrdering());
        }
        
        private interface IStreamContentWithFormParameter { [PostMethod] void Post([FormParam] IStreamContent streamContent); }
        
        [Test]
        public async Task Build_StreamContentWithFormParameter_NotThrow()
        {
            const string contentType = "application/octet-stream";
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var expectedContent = Serializer.Serialize(entity);
            var streamContent = new StreamContent(
                new MemoryStream(RequestEncoding.GetBytes(expectedContent)), 
                RequestEncoding, 
                contentType);
            
            var request = BuildRequest(
                BuildMethod<IStreamContentWithFormParameter>(), streamContent);
            
            request.Id.Should().Be(RequestId);
            request.Type.Should().Be(RequestType.Create);
            request.Resource.Should().Be(RequestUri);
            request.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            { 
                AcceptMetadata
            }, opts => opts.WithoutStrictOrdering());
            
            request.Content.Should().NotBeNull();
            request.Content!.Encoding.Should().Be(RequestEncoding);
            (await request.Content.Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent);
            request.Content.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                ContentEncodingMetadata,
                GetContentTypeMetadata(contentType),
                GetContentDispositionMetadata("attachment; name=\"\"")
            }, opts => opts.WithoutStrictOrdering());
        }
        
        private interface IFormFileWithFormParameter { [PostMethod] void Post([FormParam] IFormFile formFile); }
        
        [Test]
        public async Task Build_FormFileWithFormParameter_NotThrow()
        {
            const string contentType = "application/octet-stream";
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var expectedContent = Serializer.Serialize(entity);
            var formFileContent = new HttpFileContent(
                new MemoryStream(RequestEncoding.GetBytes(expectedContent)), 
                name: "test", 
                fileName: "test.txt", 
                contentType: contentType, 
                contentDisposition: "attachment");
            
            var request = BuildRequest(
                BuildMethod<IFormFileWithFormParameter>(), formFileContent);
            
            request.Id.Should().Be(RequestId);
            request.Type.Should().Be(RequestType.Create);
            request.Resource.Should().Be(RequestUri);
            request.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            { 
                AcceptMetadata
            }, opts => opts.WithoutStrictOrdering());
            
            request.Content.Should().NotBeNull();
            (await request.Content!.Stream.ReadToEndAsync(RequestEncoding))
                .Should().Be(expectedContent);
            request.Content.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
            {
                GetContentTypeMetadata(contentType),
                GetContentDispositionMetadata("attachment; name=\"test\"; filename=\"test.txt\"")
            }, opts => opts.WithoutStrictOrdering());
        }
    }
}
