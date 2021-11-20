using System;
using System.Collections;
using System.Text;
using FluentAssertions;
using MessagePack;
using NClient.Providers.Serialization.MessagePack.Tests.Models;
using NUnit.Framework;

namespace NClient.Providers.Serialization.MessagePack.Tests
{
    [Parallelizable]
    public class MessagePackSerializerTest
    {
        public static readonly IEnumerable SerializationValidTestCases = new[]
        {
            new TestCaseData(new Point(1, 2), new Point(1, -1), typeof(Point)),
            new TestCaseData(new Point(2, 2), new Point(2, -1), typeof(Point)),
            new TestCaseData(new Dot(1, 2), new Dot(1, 2), typeof(Dot)),
            new TestCaseData(new Dot(2, 2), new Dot(2, 2), typeof(Dot))
        };
        
        public static readonly IEnumerable DeserializationValidTestCases = new[]
        {
            new TestCaseData(new Point(1, 2), new Point(1, -1), typeof(Point)),
            new TestCaseData(new Dot(1, 2), new Dot(1, 2), typeof(Dot))
        };

        public static readonly IEnumerable NotValidTestCases = new[]
        {
            new TestCaseData(new NotMP(1, 2), new NotMP(1, 2), typeof(NotMP))
        };

        [TestCaseSource(nameof(SerializationValidTestCases))]
        public void Serialize_ValidValues_NotThrow(object obj, object expectedResult, Type type)
        {
            var serializer = new MessagePackSerializerProvider().Create(logger: null);

            var serializerString = serializer.Serialize(obj);
            byte[] bytes = new byte[serializerString.Length * sizeof(char)];
            Buffer.BlockCopy(serializerString.ToCharArray(), 0, bytes, 0, bytes.Length);
            
            var actualResult = MessagePackSerializer.Deserialize(type, bytes, MessagePackSerializerOptions.Standard);
            
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
        
        [TestCaseSource(nameof(NotValidTestCases))]
        public void Serialize_NotValidValues_Throw(object obj, object expectedResult, Type type)
        {
            var serializer = new MessagePackSerializerProvider().Create(logger: null);
            serializer
                .Invoking(x => x.Serialize(obj))
                .Should()
                .Throw<Exception>();
        }
        
        [Test]
        public void Serialize_Null_ThrowArgumentNullException()
        {
            var serializer = new MessagePackSerializerProvider().Create(logger: null);

            serializer
                .Invoking(x => x.Serialize((int?) null))
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestCaseSource(nameof(DeserializationValidTestCases))]
        public void Deserialize_ValidValues_NotThrow(object sourceObject, object expectedResult, Type type)
        {
            var serializer = new MessagePackSerializerProvider().Create(logger: null);

            var serializedBytes = MessagePackSerializer.Serialize(sourceObject, MessagePackSerializerOptions.Standard);
            
            var chars = new char[serializedBytes.Length];
            Buffer.BlockCopy(serializedBytes, 0, chars, 0, serializedBytes.Length);
            var serializedString = new string(chars);
            
            var actualResult = serializer.Deserialize(serializedString, type);
            
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
        
        [Test]
        public void Deserialize_Null_ThrowArgumentNullException()
        {
            var serializer = new MessagePackSerializerProvider().Create(logger: null);

            serializer
                .Invoking(x => x.Deserialize(null!, typeof(string)))
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
    }
}
