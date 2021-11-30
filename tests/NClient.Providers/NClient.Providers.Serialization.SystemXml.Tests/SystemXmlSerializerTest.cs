using System;
using System.Collections;
using FluentAssertions;
using NUnit.Framework;

namespace NClient.Providers.Serialization.SystemXml.Tests
{
    [Parallelizable]
    public class SystemXmlSerializerTest
    {
        public static readonly IEnumerable SerializationValidTestCases = new[]
        {
            new TestCaseData(1, "<?xml version=\"1.0\" encoding=\"utf-8\"?><int>1</int>"),
            new TestCaseData("test", "<?xml version=\"1.0\" encoding=\"utf-8\"?><string>test</string>")
        };
        
        public static readonly IEnumerable DeserializationValidTestCases = new[]
        {
            new TestCaseData("<?xml version=\"1.0\" encoding=\"utf-8\"?><int>1</int>", typeof(int), 1),
            new TestCaseData("<?xml version=\"1.0\" encoding=\"utf-8\"?><string>test</string>", typeof(string), "test")
        };

        [TestCaseSource(nameof(SerializationValidTestCases))]
        public void Serialize_ValidValues_NotThrow(object? obj, string expectedResult)
        {
            var serializer = new SystemXmlSerializerProvider().Create(logger: null);

            var actualResult = serializer.Serialize(obj);

            actualResult.Should().Be(expectedResult);
        }
        
        [Test]
        public void Serialize_Null_ThrowArgumentNullException()
        {
            var serializer = new SystemXmlSerializerProvider().Create(logger: null);

            serializer
                .Invoking(x => x.Serialize((int?) null))
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestCaseSource(nameof(DeserializationValidTestCases))]
        public void Deserialize_ValidValues_NotThrow(string xml, Type type, object? expectedResult)
        {
            var serializer = new SystemXmlSerializerProvider().Create(logger: null);

            var actualResult = serializer.Deserialize(xml, type);

            actualResult.Should().Be(expectedResult);
        }
        
        [Test]
        public void Deserialize_Null_ThrowArgumentNullException()
        {
            var serializer = new SystemXmlSerializerProvider().Create(logger: null);

            serializer
                .Invoking(x => x.Deserialize(null!, typeof(string)))
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
    }
}
