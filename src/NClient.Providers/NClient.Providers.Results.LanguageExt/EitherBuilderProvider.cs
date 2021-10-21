using NClient.Abstractions.Providers.HttpClient;
using NClient.Abstractions.Providers.Results;

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
