using NClient.Providers.Transport.Common;
using System.Threading;

namespace NClient.Standalone.ClientProxy.Validation
{
    internal class PipelineCanceller : IPipelineCanceller
    {
        internal CancellationTokenSource _canceller;
        public PipelineCanceller()
        {
            _canceller = new CancellationTokenSource();
        }
        public void Renew()
        {
            _canceller.Dispose();
            _canceller = new CancellationTokenSource();
        }
        public void Cancel()
        {
            _canceller.Cancel();
        }

        public bool IsCancellationRequested
        {
            get
            {
                return _canceller.IsCancellationRequested;
            }
        }

        public void Dispose()
        {
            _canceller.Dispose();
        }
    }
}
