


using NClient.Providers.Transport;

namespace NClient.Providers.Results
{
    public class ResultBuilderProvider : IResultBuilderProvider<IHttpResponse>
    {
        public IResultBuilder<IHttpResponse> Create()
        {
            return new ResultBuilder();
        }
    }
}
