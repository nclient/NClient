using System.Xml;
using NClient.Common.Helpers;
using NClient.Providers.Serialization.SystemXml;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SystemXmlSerializationExtensions
    {
        /// <summary>Sets the System.Xml.XmlSerializer serializer for the client.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSystemXmlSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));

            return optionalBuilder.WithCustomSerialization(new SystemXmlSerializerProvider());
        }

        /// <summary>Sets the System.Xml.XmlSerializer serializer for the client.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSystemXmlSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));

            return optionalBuilder.WithCustomSerialization(new SystemXmlSerializerProvider(xmlReaderSettings, xmlWriterSettings));
        }

        /// <summary>Sets the System.Xml.XmlSerializer serializer for the client.</summary>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));

            return optionalBuilder.WithCustomSerialization(new SystemXmlSerializerProvider());
        }

        /// <summary>Sets the System.Xml.XmlSerializer serializer for the client.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="xmlReaderSettings">The settings to be used with <see cref="XmlReader"/>.</param>
        /// <param name="xmlWriterSettings">The settings to be used with <see cref="XmlWriter"/>.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSystemXmlSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            XmlReaderSettings xmlReaderSettings, XmlWriterSettings xmlWriterSettings)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));

            return optionalBuilder.WithCustomSerialization(new SystemXmlSerializerProvider(xmlReaderSettings, xmlWriterSettings));
        }
    }
}
