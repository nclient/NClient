using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Mapping
{
    public class ResponseToStreamMapperProvider : IResponseMapperProvider<IRequest, IResponse>
    {
        public IResponseMapper<IRequest, IResponse> Create(IToolset toolset)
        {
            return new ResponseToStreamMapper();
        }
    }
}
