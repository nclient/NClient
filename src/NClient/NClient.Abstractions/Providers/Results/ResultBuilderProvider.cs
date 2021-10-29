using NClient.Providers.Transport;

namespace NClient.Providers.Results
{
    public class ResultBuilderProvider : IResultBuilderProvider<IRequest, IResponse>
    {
        public IResultBuilder<IRequest, IResponse> Create()
        {
            return new ResultBuilder();
        }
    }
}
