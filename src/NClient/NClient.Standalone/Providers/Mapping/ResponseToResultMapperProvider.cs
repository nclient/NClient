using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Mapping.Results
{
    public class ResponseToResultMapperProvider : IResponseMapperProvider<IRequest, IResponse>
    {
        public IResponseMapper<IRequest, IResponse> Create(IToolSet toolSet)
        {
            return new ResponseToResultMapper();
        }
    }
}
