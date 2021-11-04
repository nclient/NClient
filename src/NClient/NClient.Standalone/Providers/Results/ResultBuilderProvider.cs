using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Results
{
    // TODO: rename
    public class ResultBuilderProvider : IResultBuilderProvider<IRequest, IResponse>
    {
        public IResultBuilder<IRequest, IResponse> Create()
        {
            return new ResultBuilder();
        }
    }
}
