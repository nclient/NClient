using System.Xml;
using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.Xml.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SerializerBuilderExtensions
    {
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializerBuilder"></param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingSystemXmlSerializer<TClient, TRequest, TResponse>(
            this INClientSerializerBuilder<TClient, TRequest, TResponse> clientSerializerBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientSerializerBuilder, nameof(clientSerializerBuilder));

            return clientSerializerBuilder.AsAdvanced()
                .UsingCustomSerializer(new SystemXmlSerializerProvider())
                .AsBasic();
        }
        
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="factorySerializerBuilder"></param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingSystemXmlSerializer<TRequest, TResponse>(
            this INClientFactorySerializerBuilder<TRequest, TResponse> factorySerializerBuilder)
        {
            Ensure.IsNotNull(factorySerializerBuilder, nameof(factorySerializerBuilder));

            return factorySerializerBuilder.UsingCustomSerializer(new SystemXmlSerializerProvider());
        }
        
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializerBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingSystemXmlSerializer<TClient, TRequest, TResponse>(
            this INClientSerializerBuilder<TClient, TRequest, TResponse> clientSerializerBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
            where TClient : class
        {
            Ensure.IsNotNull(clientSerializerBuilder, nameof(clientSerializerBuilder));

            return clientSerializerBuilder.AsAdvanced()
                .UsingCustomSerializer(new SystemXmlSerializerProvider(xmlReaderSettings, xmlWriterSettings))
                .AsBasic();
        }
        
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="factorySerializerBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingSystemXmlSerializer<TRequest, TResponse>(
            this INClientFactorySerializerBuilder<TRequest, TResponse> factorySerializerBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
        {
            Ensure.IsNotNull(factorySerializerBuilder, nameof(factorySerializerBuilder));

            return factorySerializerBuilder.UsingCustomSerializer(new SystemXmlSerializerProvider(xmlReaderSettings, xmlWriterSettings));
        }
    }
}
