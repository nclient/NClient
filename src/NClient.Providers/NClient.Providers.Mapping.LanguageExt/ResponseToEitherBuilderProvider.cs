using NClient.Providers.Transport;

namespace NClient.Providers.Mapping.LanguageExt
{
    public class ResponseToEitherBuilderProvider : IResponseMapperProvider<IRequest, IResponse>
    {
        public IResponseMapper<IRequest, IResponse> Create(IToolSet toolSet)
        {
            return new ResponseToEitherBuilder();
        }
    }
}
