using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Results;

namespace NClient.Providers.Results.LanguageExt
{
    public class EitherBuilderProvider : IResultBuilderProvider<IHttpResponse>
    {
        public IResultBuilder<IHttpResponse> Create()
        {
            return new EitherBuilder();
        }
    }
}
