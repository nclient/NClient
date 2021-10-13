using NClient.Abstractions.HttpClients;

namespace NClient.Abstractions.Results
{
    public class ResultBuilderProvider : IResultBuilderProvider<IHttpResponse>
    {
        public IResultBuilder<IHttpResponse> Create()
        {
            return new ResultBuilder();
        }
    }
}
