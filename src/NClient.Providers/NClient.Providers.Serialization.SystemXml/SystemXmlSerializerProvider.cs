using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace NClient.Providers.Serialization.SystemXml
{
    /// <summary>The System.Xml.XmlSerializer based provider for a component that can create <see cref="ISerializer"/> instances.</summary>
    public class SystemXmlSerializerProvider : ISerializerProvider
    {
        private readonly XmlReaderSettings _xmlReaderSettings;
        private readonly XmlWriterSettings _xmlWriterSettings;
        
        /// <summary>Initializes the System.Xml.XmlSerializer based serializer provider.</summary>
        public SystemXmlSerializerProvider() : this(
            xmlReaderSettings: new XmlReaderSettings(),
            xmlWriterSettings: new XmlWriterSettings { Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false) })
        {
        }

        /// <summary>Initializes the System.Xml.XmlSerializer based serializer provider.</summary>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public SystemXmlSerializerProvider(XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
        {
            _xmlReaderSettings = xmlReaderSettings;
            _xmlWriterSettings = xmlWriterSettings;
        }

        /// <summary>Creates System.Xml.XmlSerializer <see cref="ISerializer"/> instance.</summary>
        /// <param name="logger">Optional logger. If it is not passed, then logs will not be written.</param>
        public ISerializer Create(ILogger? logger)
        {
            return new SystemXmlSerializer(_xmlReaderSettings, _xmlWriterSettings);
        }
    }
}
