using NClient.Providers.Transport;

namespace NClient.Providers.Results
{
    public class ResultBuilderProvider : IResultBuilderProvider<IResponse>
    {
        public IResultBuilder<IResponse> Create()
        {
            return new ResultBuilder();
        }
    }
}
