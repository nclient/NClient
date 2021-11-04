using System.Xml;
using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.Xml.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class OptionalBuilderExtensions
    {
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSystemXmlSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));

            return clientOptionalBuilder.AsAdvanced()
                .WithCustomSerialization(new SystemXmlSerializerProvider())
                .AsBasic();
        }
        
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));

            return factoryOptionalBuilder.WithCustomSerialization(new SystemXmlSerializerProvider());
        }
        
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSystemXmlSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));

            return clientOptionalBuilder.AsAdvanced()
                .WithCustomSerialization(new SystemXmlSerializerProvider(xmlReaderSettings, xmlWriterSettings))
                .AsBasic();
        }
        
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));

            return factoryOptionalBuilder.WithCustomSerialization(new SystemXmlSerializerProvider(xmlReaderSettings, xmlWriterSettings));
        }
    }
}
