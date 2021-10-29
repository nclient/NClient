using NClient.Providers.Transport;

namespace NClient.Providers.Results.LanguageExt
{
    public class EitherBuilderProvider : IResultBuilderProvider<IRequest, IResponse>
    {
        public IResultBuilder<IRequest, IResponse> Create()
        {
            return new EitherBuilder();
        }
    }
}
