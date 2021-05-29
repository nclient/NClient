using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.OptionalNClientBuilders.Bases;

namespace NClient.OptionalNClientBuilders
{
    internal class OptionalNClientFactoryBuilder :
        OptionalNClientBuilderBase<IOptionalNClientFactoryBuilder, INClientFactory>,
        IOptionalNClientFactoryBuilder
    {
        public OptionalNClientFactoryBuilder(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider)
            : base(httpClientProvider, serializerProvider)
        {
        }

        public override INClientFactory Build()
        {
            return new NClientStandaloneFactory(
                HttpClientProvider,
                SerializerProvider,
                ResiliencePolicyProvider,
                LoggerFactory);
        }
    }
}