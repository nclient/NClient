using NClient.Providers.Transport;

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
