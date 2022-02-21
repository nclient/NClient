using System.Xml;
using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.SystemXml;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UsingSystemXmlSerializationExtensions
    {
        /// <summary>Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingSystemXmlSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> serializationBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingCustomSerializer(new SystemXmlSerializerProvider());
        }

        /// <summary>Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.</summary>
        /// <param name="serializationBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingSystemXmlSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> serializationBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
            where TClient : class
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));
            Ensure.IsNotNull(xmlReaderSettings, nameof(xmlReaderSettings));
            Ensure.IsNotNull(xmlWriterSettings, nameof(xmlWriterSettings));

            return serializationBuilder.UsingCustomSerializer(new SystemXmlSerializerProvider(xmlReaderSettings, xmlWriterSettings));
        }

        /// <summary>Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.</summary>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> serializationBuilder)
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingCustomSerializer(new SystemXmlSerializerProvider());
        }

        /// <summary>Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.</summary>
        /// <param name="serializationBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> serializationBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));
            Ensure.IsNotNull(xmlReaderSettings, nameof(xmlReaderSettings));
            Ensure.IsNotNull(xmlWriterSettings, nameof(xmlWriterSettings));

            return serializationBuilder.UsingCustomSerializer(new SystemXmlSerializerProvider(xmlReaderSettings, xmlWriterSettings));
        }
    }
}
