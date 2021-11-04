using System.Xml;
using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.Xml.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SystemXmlSerializationExtensions
    {
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedOptionalBuilder"></param>
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithSystemXmlSerialization<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));

            return clientAdvancedOptionalBuilder.WithCustomSerialization(new SystemXmlSerializerProvider());
        }
        
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSystemXmlSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));

            return WithSystemXmlSerialization(clientOptionalBuilder.AsAdvanced()).AsBasic();
        }

        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedOptionalBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithSystemXmlSerialization<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));

            return clientAdvancedOptionalBuilder.WithCustomSerialization(new SystemXmlSerializerProvider(xmlReaderSettings, xmlWriterSettings));
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

            return WithSystemXmlSerialization(clientOptionalBuilder.AsAdvanced(), xmlReaderSettings, xmlWriterSettings).AsBasic();
        }
        
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedOptionalBuilder"></param>
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> clientAdvancedOptionalBuilder)
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));

            return clientAdvancedOptionalBuilder.WithCustomSerialization(new SystemXmlSerializerProvider());
        }
        
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> clientOptionalBuilder)
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));

            return WithSystemXmlSerialization(clientOptionalBuilder.AsAdvanced()).AsBasic();
        }

        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedOptionalBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> clientAdvancedOptionalBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));

            return clientAdvancedOptionalBuilder.WithCustomSerialization(new SystemXmlSerializerProvider(xmlReaderSettings, xmlWriterSettings));
        }
        
        /// <summary>
        /// Sets System.Xml.XmlSerializer based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> clientOptionalBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));

            return WithSystemXmlSerialization(clientOptionalBuilder.AsAdvanced(), xmlReaderSettings, xmlWriterSettings).AsBasic();
        }
    }
}
