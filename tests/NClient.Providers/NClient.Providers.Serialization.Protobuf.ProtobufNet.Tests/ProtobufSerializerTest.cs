using System;
using System.Collections;
using System.IO;
using FluentAssertions;
using NClient.Common.Helpers;
using NClient.Providers.Serialization.Protobuf.ProtobufNet.Tests.Models;
using NUnit.Framework;
using ProtoBuf;

namespace NClient.Providers.Serialization.Protobuf.ProtobufNet.Tests
{
    [Parallelizable]
    public class ProtobufSerializerTest
    {
        public static readonly IEnumerable SerializationValidTestCases = new[]
        {
            new TestCaseData(new Point { X = 1, Y = 2 }, new Point { X = 1, Y = 2 }, typeof(Point)),
            new TestCaseData(new Point { X = 2, Y = 2 }, new Point { X = 2, Y = 2 }, typeof(Point))
        };

        public static readonly IEnumerable DeserializationValidTestCases = new[]
        {
            new TestCaseData(new Point { X = 1, Y = 2 }, new Point { X = 1, Y = 2 }, typeof(Point))
        };

        public static readonly IEnumerable NotValidTestCases = new[]
        {
            new TestCaseData(new NotProto(1, 2), new NotProto(1, 2), typeof(NotProto))
        };

        [TestCaseSource(nameof(SerializationValidTestCases))]
        public void Serialize_ValidValues_NotThrow(object obj, object expectedResult, Type type)
        {
            var serializer = new ProtobufSerializerProvider().Create(logger: null);

            var serializerString = serializer.Serialize(obj);
            byte[] bytes = Converters.GetBytes(serializerString);

            var actualResult = Serializer.Deserialize(type, new MemoryStream(bytes));

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
        
        [Test]
        public void Serialize_Null_ThrowArgumentNullException()
        {
            var serializer = new ProtobufSerializerProvider().Create(logger: null);

            serializer
                .Invoking(x => x.Serialize((int?) null))
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }

        [TestCaseSource(nameof(DeserializationValidTestCases))]
        public void Deserialize_ValidValues_NotThrow(object sourceObject, object expectedResult, Type type)
        {
            var serializer = new ProtobufSerializerProvider().Create(logger: null);

            using var memoryStream = new MemoryStream();
            Serializer.Serialize(memoryStream, sourceObject);
            
            var serializedBytes = memoryStream.ToArray();
            
            var serializedString = Converters.GetString(serializedBytes);
            
            var actualResult = serializer.Deserialize(serializedString, type);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void Deserialize_Null_ThrowArgumentNullException()
        {
            var serializer = new ProtobufSerializerProvider().Create(logger: null);

            serializer
                .Invoking(x => x.Deserialize(null!, typeof(string)))
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
    }
}
