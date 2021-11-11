using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace NClient.Providers.Serialization.Xml.System
{
    /// <summary>
    /// System.Xml.XmlSerializer based provider for a component that can create <see cref="ISerializer"/> instances.
    /// </summary>
    public class SystemXmlSerializerProvider : ISerializerProvider
    {
        private readonly XmlReaderSettings _xmlReaderSettings;
        private readonly XmlWriterSettings _xmlWriterSettings;
        
        /// <summary>
        /// Creates the System.Xml.XmlSerializer based serializer provider.
        /// </summary>
        public SystemXmlSerializerProvider() : this(
            xmlReaderSettings: new XmlReaderSettings(),
            xmlWriterSettings: new XmlWriterSettings { Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false) })
        {
        }

        /// <summary>
        /// Creates the System.Xml.XmlSerializer based serializer provider.
        /// </summary>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public SystemXmlSerializerProvider(XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
        {
            _xmlReaderSettings = xmlReaderSettings;
            _xmlWriterSettings = xmlWriterSettings;
        }

        public ISerializer Create(ILogger? logger)
        {
            return new SystemXmlSerializer(_xmlReaderSettings, _xmlWriterSettings);
        }
    }
}
