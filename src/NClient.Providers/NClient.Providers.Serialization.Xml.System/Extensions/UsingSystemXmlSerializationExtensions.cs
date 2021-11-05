using System.Xml;
using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.Xml.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UsingSystemXmlSerializationExtensions
    {
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializationBuilder"></param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingSystemXmlSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> clientAdvancedSerializationBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedSerializationBuilder, nameof(clientAdvancedSerializationBuilder));

            return clientAdvancedSerializationBuilder.UsingCustomSerializer(new SystemXmlSerializerProvider());
        }

        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializationBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingSystemXmlSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> clientAdvancedSerializationBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedSerializationBuilder, nameof(clientAdvancedSerializationBuilder));
            Ensure.IsNotNull(xmlReaderSettings, nameof(xmlReaderSettings));
            Ensure.IsNotNull(xmlWriterSettings, nameof(xmlWriterSettings));

            return clientAdvancedSerializationBuilder.UsingCustomSerializer(new SystemXmlSerializerProvider(xmlReaderSettings, xmlWriterSettings));
        }

        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializationBuilder"></param>
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> UsingSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactoryAdvancedSerializationBuilder<TRequest, TResponse> clientAdvancedSerializationBuilder)
        {
            Ensure.IsNotNull(clientAdvancedSerializationBuilder, nameof(clientAdvancedSerializationBuilder));

            return clientAdvancedSerializationBuilder.UsingCustomSerializer(new SystemXmlSerializerProvider());
        }
        
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializationBuilder"></param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> clientSerializationBuilder)
        {
            Ensure.IsNotNull(clientSerializationBuilder, nameof(clientSerializationBuilder));

            return UsingSystemXmlSerialization(clientSerializationBuilder.AsAdvanced()).AsBasic();
        }

        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializationBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> UsingSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactoryAdvancedSerializationBuilder<TRequest, TResponse> clientAdvancedSerializationBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
        {
            Ensure.IsNotNull(clientAdvancedSerializationBuilder, nameof(clientAdvancedSerializationBuilder));
            Ensure.IsNotNull(xmlReaderSettings, nameof(xmlReaderSettings));
            Ensure.IsNotNull(xmlWriterSettings, nameof(xmlWriterSettings));

            return clientAdvancedSerializationBuilder.UsingCustomSerializer(new SystemXmlSerializerProvider(xmlReaderSettings, xmlWriterSettings));
        }
        
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializationBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> clientSerializationBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
        {
            Ensure.IsNotNull(clientSerializationBuilder, nameof(clientSerializationBuilder));
            Ensure.IsNotNull(xmlReaderSettings, nameof(xmlReaderSettings));
            Ensure.IsNotNull(xmlWriterSettings, nameof(xmlWriterSettings));

            return UsingSystemXmlSerialization(clientSerializationBuilder.AsAdvanced(), xmlReaderSettings, xmlWriterSettings).AsBasic();
        }
    }
}
