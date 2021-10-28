using NClient.Providers.Transport;

namespace NClient.Providers.Results.LanguageExt
{
    public class EitherBuilderProvider : IResultBuilderProvider<IResponse>
    {
        public IResultBuilder<IResponse> Create()
        {
            return new EitherBuilder();
        }
    }
}
