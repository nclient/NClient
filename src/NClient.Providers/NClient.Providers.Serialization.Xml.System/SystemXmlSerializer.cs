using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.Xml.System
{
    internal class SystemXmlSerializer : ISerializer
    {
        private readonly XmlReaderSettings _xmlReaderSettings;
        private readonly XmlWriterSettings _xmlWriterSettings;
        
        public string ContentType { get; } = "application/xml";

        public SystemXmlSerializer(XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
        {
            _xmlReaderSettings = xmlReaderSettings;
            _xmlWriterSettings = xmlWriterSettings;
        }
        
        public object? Deserialize(string source, Type returnType)
        {
            Ensure.IsNotNull(source, nameof(source));
            Ensure.IsNotNull(returnType, nameof(returnType));

            using var stringReader = new StringReader(source);
            using var xmlReader = XmlReader.Create(stringReader, _xmlReaderSettings);
            return new XmlSerializer(returnType).Deserialize(xmlReader);
        }

        public string Serialize<T>(T? value)
        {
            Ensure.IsNotNull((object)value!, nameof(value));
            
            using var memoryStream = new MemoryStream();
            using var xmlWriter = XmlWriter.Create(memoryStream, _xmlWriterSettings);
            new XmlSerializer(value.GetType()).Serialize(xmlWriter, value);
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }
}
